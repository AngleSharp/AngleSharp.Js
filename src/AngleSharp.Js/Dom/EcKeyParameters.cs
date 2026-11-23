namespace AngleSharp.Js.Dom
{
    using System;
    using System.Security.Cryptography;
#if NETFRAMEWORK
    using Microsoft.Win32.SafeHandles;
    using System.Runtime.InteropServices;
#endif

    /// <summary>
    /// The material of an elliptic curve key on one of the named curves of the Web Cryptography
    /// API. It stands in for ECParameters, which .NET Framework 4.6.2 does not have: on .NET
    /// Framework the key reaches the platform (CNG) as a key blob instead.
    /// </summary>
    readonly struct EcKeyParameters
    {
        public EcKeyParameters(String curve, Byte[] x, Byte[] y, Byte[] d)
        {
            Curve = curve;
            X = x;
            Y = y;
            D = d;
        }

        /// <summary>
        /// Gets the name of the curve, i.e., "P-256", "P-384" or "P-521".
        /// </summary>
        public String Curve { get; }

        public Byte[] X { get; }

        public Byte[] Y { get; }

        /// <summary>
        /// Gets the private key, which is null for a public key.
        /// </summary>
        public Byte[] D { get; }

        public Boolean IsPrivate => D != null;

        public EcKeyParameters PublicPart => new EcKeyParameters(Curve, X, Y, null);

        public static Int32 CoordinateLength(String curve) =>
            curve == "P-256" ? 32 : curve == "P-384" ? 48 : 66;

        /// <summary>
        /// Creates a new key pair on the given curve.
        /// </summary>
        public static EcKeyParameters Generate(String curve)
        {
#if NETFRAMEWORK
            //  the key has to allow the export explicitly, as its private part is taken out of it
            var creation = new CngKeyCreationParameters { ExportPolicy = CngExportPolicies.AllowPlaintextExport };

            using (var key = CngKey.Create(GetCngAlgorithm(curve), null, creation))
            {
                return FromBlob(curve, key.Export(CngKeyBlobFormat.EccPrivateBlob));
            }
#else
            using (var ecdsa = ECDsa.Create(GetNamedCurve(curve)))
            {
                return From(ecdsa.ExportParameters(true)).Value;
            }
#endif
        }

        /// <summary>
        /// Creates the platform's ECDSA for the key, which also checks the key (e.g. that the
        /// point is on the curve) by throwing a CryptographicException.
        /// </summary>
        public ECDsa CreateEcdsa()
        {
#if NETFRAMEWORK
            //  ECDsaCng duplicates the handle of the key it is given
            using (var key = ImportCngKey(false))
            {
                return new ECDsaCng(key);
            }
#else
            return ECDsa.Create(ToParameters());
#endif
        }

#if NET8_0_OR_GREATER || NETFRAMEWORK
        /// <summary>
        /// Derives the shared secret of ECDH (the x coordinate of the product, not hashed) of
        /// this private key and the given public key. Throws a PlatformNotSupportedException
        /// if the platform can't give out the secret as it is.
        /// </summary>
        public Byte[] DeriveSecret(EcKeyParameters publicKey)
        {
#if NETFRAMEWORK
            //  ECDiffieHellmanCng only gives out secrets put through a KDF, but CNG itself gives
            //  out the raw one (BCRYPT_KDF_RAW_SECRET, since Windows 10) - which is how .NET
            //  implements DeriveRawSecretAgreement on Windows
            using (var mine = ImportCngKey(true))
            using (var theirs = publicKey.ImportCngKey(true))
            using (var ecdh = new ECDiffieHellmanCng(mine))
            using (var secret = ecdh.DeriveSecretAgreementHandle(theirs))
            {
                //  asked for the length first, then for the secret
                var status = NCryptDeriveKey(secret, RawSecretKdf, IntPtr.Zero, null, 0, out var length, 0);
                var result = new Byte[status == 0 ? length : 0];

                if (status == 0)
                {
                    status = NCryptDeriveKey(secret, RawSecretKdf, IntPtr.Zero, result, result.Length, out length, 0);
                }

                if (status == NotSupportedStatus || status == InvalidParameterStatus)
                {
                    throw new PlatformNotSupportedException();
                }
                else if (status != 0)
                {
                    throw new CryptographicException(status);
                }

                //  CNG gives out the secret as a little endian number
                Array.Reverse(result, 0, length);
                return length == result.Length ? result : Slice(result, 0, length);
            }
#else
            using (var mine = ECDiffieHellman.Create(ToParameters()))
            using (var theirs = ECDiffieHellman.Create(publicKey.ToParameters()))
            {
                return mine.DeriveRawSecretAgreement(theirs.PublicKey);
            }
#endif
        }
#endif

#if NETFRAMEWORK
        private const String RawSecretKdf = "TRUNCATE";

        private const Int32 NotSupportedStatus = unchecked((Int32)0x80090029);

        private const Int32 InvalidParameterStatus = unchecked((Int32)0x80090027);

        //  BCRYPT_ECCKEY_BLOB: the magic and the length of a coordinate (each a little endian
        //  UInt32), followed by X, Y and - for a private key - D, each of that length.
        private const Int32 BlobHeaderLength = 8;

        [DllImport("ncrypt.dll", CharSet = CharSet.Unicode)]
        private static extern Int32 NCryptDeriveKey(SafeNCryptSecretHandle hSharedSecret, String pwszKDF, IntPtr pParameterList, Byte[] pbDerivedKey, Int32 cbDerivedKey, out Int32 pcbResult, Int32 dwFlags);

        private CngKey ImportCngKey(Boolean isEcdh)
        {
            var format = IsPrivate ? CngKeyBlobFormat.EccPrivateBlob : CngKeyBlobFormat.EccPublicBlob;
            return CngKey.Import(ToBlob(isEcdh), format);
        }

        private Byte[] ToBlob(Boolean isEcdh)
        {
            var size = CoordinateLength(Curve);
            var count = IsPrivate ? 3 : 2;
            var blob = new Byte[BlobHeaderLength + count * size];
            WriteUInt32(blob, 0, GetMagic(Curve, isEcdh, IsPrivate));
            WriteUInt32(blob, 4, (UInt32)size);
            CopyPadded(X, blob, BlobHeaderLength, size);
            CopyPadded(Y, blob, BlobHeaderLength + size, size);

            if (IsPrivate)
            {
                CopyPadded(D, blob, BlobHeaderLength + 2 * size, size);
            }

            return blob;
        }

        private static EcKeyParameters FromBlob(String curve, Byte[] blob)
        {
            var size = (Int32)(blob[4] | blob[5] << 8 | blob[6] << 16 | blob[7] << 24);
            var hasD = blob.Length >= BlobHeaderLength + 3 * size;
            var x = Slice(blob, BlobHeaderLength, size);
            var y = Slice(blob, BlobHeaderLength + size, size);
            var d = hasD ? Slice(blob, BlobHeaderLength + 2 * size, size) : null;
            return new EcKeyParameters(curve, x, y, d);
        }

        //  "ECS1" to "ECS6" for ECDSA and "ECK1" to "ECK6" for ECDH: public and private of each
        //  curve in turn, as little endian UInt32 values.
        private static UInt32 GetMagic(String curve, Boolean isEcdh, Boolean isPrivate)
        {
            var index = curve == "P-256" ? 1u : curve == "P-384" ? 3u : 5u;
            return (isEcdh ? 0x304B4345u : 0x30534345u) + index + (isPrivate ? 1u : 0u);
        }

        private static CngAlgorithm GetCngAlgorithm(String curve)
        {
            switch (curve)
            {
                case "P-256":
                    return CngAlgorithm.ECDsaP256;
                case "P-384":
                    return CngAlgorithm.ECDsaP384;
                default:
                    return CngAlgorithm.ECDsaP521;
            }
        }

        private static void WriteUInt32(Byte[] target, Int32 offset, UInt32 value)
        {
            target[offset] = (Byte)value;
            target[offset + 1] = (Byte)(value >> 8);
            target[offset + 2] = (Byte)(value >> 16);
            target[offset + 3] = (Byte)(value >> 24);
        }

        //  Right-aligned, as the coordinates are big endian numbers.
        private static void CopyPadded(Byte[] value, Byte[] target, Int32 offset, Int32 size)
        {
            var start = 0;

            while (value.Length - start > size && value[start] == 0)
            {
                start++;
            }

            var length = value.Length - start;

            if (length > size)
            {
                throw new CryptographicException();
            }

            Buffer.BlockCopy(value, start, target, offset + size - length, length);
        }

        private static Byte[] Slice(Byte[] value, Int32 offset, Int32 length)
        {
            var result = new Byte[length];
            Buffer.BlockCopy(value, offset, result, 0, length);
            return result;
        }
#else
        public ECParameters ToParameters() => new ECParameters
        {
            Curve = GetNamedCurve(Curve),
            Q = new ECPoint { X = X, Y = Y },
            D = D,
        };

        /// <summary>
        /// Gets the key of the given parameters, or null if they are not on one of the named
        /// curves of the Web Cryptography API.
        /// </summary>
        public static EcKeyParameters? From(ECParameters parameters)
        {
            var curve = GetCurveName(parameters.Curve);
            return curve != null ? new EcKeyParameters(curve, parameters.Q.X, parameters.Q.Y, parameters.D) : (EcKeyParameters?)null;
        }

        private static ECCurve GetNamedCurve(String curve)
        {
            switch (curve)
            {
                case "P-256":
                    return ECCurve.NamedCurves.nistP256;
                case "P-384":
                    return ECCurve.NamedCurves.nistP384;
                default:
                    return ECCurve.NamedCurves.nistP521;
            }
        }

        private static String GetCurveName(ECCurve curve)
        {
            switch (curve.Oid?.Value)
            {
                case "1.2.840.10045.3.1.7":
                    return "P-256";
                case "1.3.132.0.34":
                    return "P-384";
                case "1.3.132.0.35":
                    return "P-521";
            }

            switch (curve.Oid?.FriendlyName)
            {
                case "nistP256":
                case "ECDSA_P256":
                    return "P-256";
                case "nistP384":
                case "ECDSA_P384":
                    return "P-384";
                case "nistP521":
                case "ECDSA_P521":
                    return "P-521";
            }

            return null;
        }
#endif
    }
}
