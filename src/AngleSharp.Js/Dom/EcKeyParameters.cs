namespace AngleSharp.Js.Dom
{
    using System;
    using System.Security.Cryptography;

    /// <summary>
    /// The material of an elliptic curve key on one of the named curves of the Web Cryptography
    /// API. It stands in for ECParameters, which .NET Framework 4.6.2 does not have: there the
    /// key reaches the platform as a CNG key blob instead.
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
#if NET462
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
#if NET462
            var format = IsPrivate ? CngKeyBlobFormat.EccPrivateBlob : CngKeyBlobFormat.EccPublicBlob;
            return new ECDsaCng(CngKey.Import(ToBlob(), format));
#else
            return ECDsa.Create(ToParameters());
#endif
        }

#if NET462
        //  BCRYPT_ECCKEY_BLOB: the magic and the length of a coordinate (each a little endian
        //  UInt32), followed by X, Y and - for a private key - D, each of that length.
        private const Int32 BlobHeaderLength = 8;

        private Byte[] ToBlob()
        {
            var size = CoordinateLength(Curve);
            var count = IsPrivate ? 3 : 2;
            var blob = new Byte[BlobHeaderLength + count * size];
            WriteUInt32(blob, 0, GetMagic(Curve, IsPrivate));
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

        //  The ECDSA magics ("ECS1" to "ECS6"): an ECDH key only ever reaches the platform to be
        //  checked here, which the ECDSA algorithm of the same curve does just as well.
        private static UInt32 GetMagic(String curve, Boolean isPrivate)
        {
            var index = curve == "P-256" ? 1u : curve == "P-384" ? 3u : 5u;
            return 0x30534345u + index + (isPrivate ? 1u : 0u);
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
