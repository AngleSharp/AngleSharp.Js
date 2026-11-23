namespace AngleSharp.Js.Dom
{
    using AngleSharp.Dom;
    using Jint;
    using Jint.Native;
    using Jint.Native.Json;
    using Jint.Native.Object;
    using Jint.Runtime;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// The operations of crypto.subtle (Web Cryptography API, W3C), done with the cryptography
    /// of .NET. AES-GCM, the formats spki and pkcs8, and ECDH need .NET 8 or later.
    /// </summary>
    static class WebCrypto
    {
        private const String Encrypt_ = "encrypt";
        private const String Decrypt_ = "decrypt";
        private const String Sign_ = "sign";
        private const String Verify_ = "verify";
        private const String DeriveKey_ = "deriveKey";
        private const String DeriveBits_ = "deriveBits";
        private const String WrapKey_ = "wrapKey";
        private const String UnwrapKey_ = "unwrapKey";
        private const String Digest_ = "digest";
        private const String GenerateKey_ = "generateKey";
        private const String ImportKey_ = "importKey";
        private const String GetKeyLength_ = "get key length";

        //  The order of the KeyUsage enumeration, which is the order of a key's usages.
        private static readonly String[] AllUsages = { Encrypt_, Decrypt_, Sign_, Verify_, DeriveKey_, DeriveBits_, WrapKey_, UnwrapKey_ };

        private static readonly Dictionary<String, String[]> Operations = new Dictionary<String, String[]>
        {
            ["SHA-1"] = new[] { Digest_ },
            ["SHA-256"] = new[] { Digest_ },
            ["SHA-384"] = new[] { Digest_ },
            ["SHA-512"] = new[] { Digest_ },
            ["HMAC"] = new[] { Sign_, Verify_, GenerateKey_, ImportKey_, GetKeyLength_ },
            ["AES-CBC"] = new[] { Encrypt_, Decrypt_, GenerateKey_, ImportKey_, GetKeyLength_ },
            ["AES-CTR"] = new[] { Encrypt_, Decrypt_, GenerateKey_, ImportKey_, GetKeyLength_ },
            ["AES-GCM"] = new[] { Encrypt_, Decrypt_, GenerateKey_, ImportKey_, GetKeyLength_ },
            ["AES-KW"] = new[] { WrapKey_, UnwrapKey_, GenerateKey_, ImportKey_, GetKeyLength_ },
            ["PBKDF2"] = new[] { DeriveBits_, ImportKey_, GetKeyLength_ },
            ["HKDF"] = new[] { DeriveBits_, ImportKey_, GetKeyLength_ },
            ["ECDSA"] = new[] { Sign_, Verify_, GenerateKey_, ImportKey_ },
            ["ECDH"] = new[] { DeriveBits_, GenerateKey_, ImportKey_ },
            ["RSASSA-PKCS1-v1_5"] = new[] { Sign_, Verify_, GenerateKey_, ImportKey_ },
            ["RSA-PSS"] = new[] { Sign_, Verify_, GenerateKey_, ImportKey_ },
            ["RSA-OAEP"] = new[] { Encrypt_, Decrypt_, GenerateKey_, ImportKey_ },
        };

        #region Operations

        public static Byte[] Digest(Object algorithm, Byte[] data)
        {
            var normalized = Normalize(algorithm, Digest_);
            return Hash(normalized.Name, Required(data));
        }

        public static Byte[] Encrypt(Object algorithm, CryptoKey key, Byte[] data)
        {
            var normalized = Normalize(algorithm, Encrypt_);
            CheckKey(key, normalized, Encrypt_);
            return EncryptCore(normalized, key, Required(data));
        }

        public static Byte[] Decrypt(Object algorithm, CryptoKey key, Byte[] data)
        {
            var normalized = Normalize(algorithm, Decrypt_);
            CheckKey(key, normalized, Decrypt_);
            return DecryptCore(normalized, key, Required(data));
        }

        public static Byte[] Sign(Object algorithm, CryptoKey key, Byte[] data)
        {
            var normalized = Normalize(algorithm, Sign_);
            CheckKey(key, normalized, Sign_);
            data = Required(data);

            switch (normalized.Name)
            {
                case "HMAC":
                    return Hmac(key.Hash, key.Secret, data);
                case "ECDSA":
                    using (var ecdsa = ECDsa.Create(key.Ec.Value))
                    {
                        return ecdsa.SignData(data, HashName(normalized.GetHash()));
                    }
                default:
                    using (var rsa = CreateRsa(key))
                    {
                        return rsa.SignData(data, HashName(key.Hash), GetRsaPadding(normalized, key));
                    }
            }
        }

        public static Boolean Verify(Object algorithm, CryptoKey key, Byte[] signature, Byte[] data)
        {
            var normalized = Normalize(algorithm, Verify_);
            CheckKey(key, normalized, Verify_);
            signature = Required(signature);
            data = Required(data);

            switch (normalized.Name)
            {
                case "HMAC":
                    return AreEqual(Hmac(key.Hash, key.Secret, data), signature);
                case "ECDSA":
                    using (var ecdsa = ECDsa.Create(key.Ec.Value))
                    {
                        //  the signature is r and s, each as long as a coordinate (IEEE P1363)
                        return signature.Length == 2 * CoordinateLength(key.Curve) &&
                            ecdsa.VerifyData(data, signature, HashName(normalized.GetHash()));
                    }
                default:
                    using (var rsa = CreateRsa(key))
                    {
                        try
                        {
                            return rsa.VerifyData(data, signature, HashName(key.Hash), GetRsaPadding(normalized, key));
                        }
                        catch (CryptographicException)
                        {
                            return false;
                        }
                    }
            }
        }

        public static Object GenerateKey(Object algorithm, Boolean extractable, String[] usages)
        {
            var normalized = Normalize(algorithm, GenerateKey_);
            usages = CheckUsageValues(usages);

            switch (normalized.Name)
            {
                case "HMAC":
                {
                    CheckUsages(usages, Sign_, Verify_);
                    var hash = normalized.GetHash();
                    var length = normalized.GetInteger("length") ?? BlockBits(hash);

                    if (length == 0)
                    {
                        throw OperationError();
                    }

                    return RequireUsages(CreateHmacKey(hash, RandomBytes((length + 7) / 8), length, extractable, usages));
                }
                case "AES-CBC":
                case "AES-CTR":
                case "AES-GCM":
                case "AES-KW":
                {
                    CheckUsages(usages, GetAesUsages(normalized.Name));
                    var length = normalized.GetInteger("length") ?? throw TypeError("The length of the key is missing.");

                    if (length != 128 && length != 192 && length != 256)
                    {
                        throw OperationError();
                    }

                    return RequireUsages(CreateAesKey(normalized.Name, RandomBytes(length / 8), extractable, usages));
                }
                case "ECDSA":
                case "ECDH":
                {
                    var isEcdsa = normalized.Name == "ECDSA";
                    CheckUsages(usages, isEcdsa ? new[] { Sign_, Verify_ } : new[] { DeriveKey_, DeriveBits_ });
                    var curve = GetCurve(normalized);
                    ECParameters parameters;

                    using (var ecdsa = ECDsa.Create(GetNamedCurve(curve)))
                    {
                        parameters = ecdsa.ExportParameters(true);
                    }

                    var publicKey = CreateEcKey(normalized.Name, curve, PublicPart(parameters), true, Intersect(usages, isEcdsa ? new[] { Verify_ } : new String[0]));
                    var privateKey = CreateEcKey(normalized.Name, curve, parameters, extractable, Intersect(usages, isEcdsa ? new[] { Sign_ } : new[] { DeriveKey_, DeriveBits_ }));
                    return CreateKeyPair(publicKey, RequireUsages(privateKey));
                }
                default:
                {
                    var (publicUsages, privateUsages) = GetRsaUsages(normalized.Name);
                    CheckUsages(usages, publicUsages.Concat(privateUsages).ToArray());
                    var bits = normalized.GetInteger("modulusLength") ?? throw TypeError("The modulusLength is missing.");
                    var exponent = normalized.GetBytes("publicExponent");
                    var hash = normalized.GetHash();

                    //  the platform only generates keys with the usual exponent 65537
                    if (!AreEqual(TrimZeros(exponent), new Byte[] { 1, 0, 1 }))
                    {
                        throw OperationError();
                    }

                    RSAParameters parameters;

                    try
                    {
                        using (var rsa = RSA.Create())
                        {
                            rsa.KeySize = bits;
                            parameters = rsa.ExportParameters(true);
                        }
                    }
                    catch (CryptographicException)
                    {
                        throw OperationError();
                    }

                    var publicKey = CreateRsaKey(normalized.Name, hash, PublicPart(parameters), true, Intersect(usages, publicUsages));
                    var privateKey = CreateRsaKey(normalized.Name, hash, parameters, extractable, Intersect(usages, privateUsages));
                    return CreateKeyPair(publicKey, RequireUsages(privateKey));
                }
            }
        }

        public static CryptoKey ImportKey(String format, Object keyData, Object algorithm, Boolean extractable, String[] usages)
        {
            CheckFormat(format);
            var normalized = Normalize(algorithm, ImportKey_);
            usages = CheckUsageValues(usages);
            Byte[] data = null;
            Jwk jwk = null;

            if (format == "jwk")
            {
                jwk = keyData is ObjectInstance obj && !obj.IsBufferSource()
                    ? new Jwk(obj)
                    : throw TypeError("A JSON Web Key has to be an object.");
                jwk.Check(extractable, usages);
            }
            else
            {
                data = keyData as Byte[] ?? throw TypeError("The key data has to be an ArrayBuffer or a view on one.");
            }

            switch (normalized.Name)
            {
                case "HMAC":
                    return RequireUsages(ImportHmacKey(format, data, jwk, normalized, extractable, usages));
                case "AES-CBC":
                case "AES-CTR":
                case "AES-GCM":
                case "AES-KW":
                    return RequireUsages(ImportAesKey(format, data, jwk, normalized.Name, extractable, usages));
                case "PBKDF2":
                case "HKDF":
                    CheckUsages(usages, DeriveKey_, DeriveBits_);

                    if (format != "raw")
                    {
                        throw NotSupportedError();
                    }
                    else if (extractable)
                    {
                        throw SyntaxError();
                    }

                    return RequireUsages(new CryptoKey("secret", false, CreateAlgorithm(normalized.Name), usages) { Secret = data });
                case "ECDSA":
                case "ECDH":
                    return RequireUsages(ImportEcKey(format, data, jwk, normalized, extractable, usages));
                default:
                    return RequireUsages(ImportRsaKey(format, data, jwk, normalized, extractable, usages));
            }
        }

        public static Object ExportKey(String format, CryptoKey key)
        {
            CheckFormat(format);

            if (key is null)
            {
                throw TypeError("The key is missing.");
            }
            else if (!key.Extractable)
            {
                throw InvalidAccessError();
            }

            switch (format)
            {
                case "raw":
                    if (key.Secret != null)
                    {
                        return (Byte[])key.Secret.Clone();
                    }
                    else if (key.Ec is ECParameters ec && key.Type == "public")
                    {
                        return EncodePoint(ec, key.Curve);
                    }

                    throw InvalidAccessError();
                case "jwk":
                    return ExportJwk(key);
                default:
                    return ExportDer(format, key);
            }
        }

        public static Byte[] DeriveBits(Object algorithm, CryptoKey baseKey, Int32? length)
        {
            var normalized = Normalize(algorithm, DeriveBits_);
            CheckKey(baseKey, normalized, DeriveBits_);
            return DeriveBitsCore(normalized, baseKey, length);
        }

        public static CryptoKey DeriveKey(Object algorithm, CryptoKey baseKey, Object derivedKeyType, Boolean extractable, String[] usages)
        {
            var normalized = Normalize(algorithm, DeriveBits_);
            Normalize(derivedKeyType, ImportKey_);
            var length = GetKeyLength(Normalize(derivedKeyType, GetKeyLength_));
            CheckKey(baseKey, normalized, DeriveKey_);
            var bits = DeriveBitsCore(normalized, baseKey, length);
            return ImportKey("raw", bits, derivedKeyType, extractable, usages);
        }

        public static Byte[] WrapKey(String format, CryptoKey key, CryptoKey wrappingKey, Object wrapAlgorithm)
        {
            CheckFormat(format);
            var normalized = NormalizeWrap(wrapAlgorithm, WrapKey_, Encrypt_);
            CheckKey(wrappingKey, normalized, WrapKey_);
            var exported = ExportKey(format, key);
            var bytes = exported as Byte[] ?? Encoding.UTF8.GetBytes(ToJson(exported));
            return normalized.Name == "AES-KW" ? AesKeyWrap(wrappingKey.Secret, bytes) : EncryptCore(normalized, wrappingKey, bytes);
        }

        public static CryptoKey UnwrapKey(String format, Byte[] wrappedKey, CryptoKey unwrappingKey, Object unwrapAlgorithm, Object unwrappedKeyAlgorithm, Boolean extractable, String[] usages)
        {
            CheckFormat(format);
            var normalized = NormalizeWrap(unwrapAlgorithm, UnwrapKey_, Decrypt_);
            Normalize(unwrappedKeyAlgorithm, ImportKey_);
            CheckKey(unwrappingKey, normalized, UnwrapKey_);
            wrappedKey = Required(wrappedKey);
            var bytes = normalized.Name == "AES-KW" ? AesKeyUnwrap(unwrappingKey.Secret, wrappedKey) : DecryptCore(normalized, unwrappingKey, wrappedKey);
            Object keyData = bytes;

            if (format == "jwk")
            {
                try
                {
                    var engine = EngineInstance.Current.Jint;
                    keyData = new JsonParser(engine).Parse(Encoding.UTF8.GetString(bytes)) as ObjectInstance;
                }
                catch (JavaScriptException)
                {
                    keyData = null;
                }

                if (keyData is null)
                {
                    throw DataError();
                }
            }

            return ImportKey(format, keyData, unwrappedKeyAlgorithm, extractable, usages);
        }

        #endregion

        #region Encryption

        private static Byte[] EncryptCore(Algorithm algorithm, CryptoKey key, Byte[] data)
        {
            switch (algorithm.Name)
            {
                case "AES-CBC":
                    using (var aes = CreateAes(key.Secret, CipherMode.CBC, PaddingMode.PKCS7))
                    using (var encryptor = aes.CreateEncryptor(key.Secret, GetAesCbcIv(algorithm)))
                    {
                        return encryptor.TransformFinalBlock(data, 0, data.Length);
                    }
                case "AES-CTR":
                    return AesCtr(algorithm, key.Secret, data);
                case "AES-GCM":
                    return AesGcmEncrypt(algorithm, key.Secret, data);
                default:
                    using (var rsa = CreateRsa(key))
                    {
                        try
                        {
                            return rsa.Encrypt(data, GetOaepPadding(algorithm, key));
                        }
                        catch (CryptographicException)
                        {
                            throw OperationError();
                        }
                    }
            }
        }

        private static Byte[] DecryptCore(Algorithm algorithm, CryptoKey key, Byte[] data)
        {
            try
            {
                switch (algorithm.Name)
                {
                    case "AES-CBC":
                        using (var aes = CreateAes(key.Secret, CipherMode.CBC, PaddingMode.PKCS7))
                        using (var decryptor = aes.CreateDecryptor(key.Secret, GetAesCbcIv(algorithm)))
                        {
                            return decryptor.TransformFinalBlock(data, 0, data.Length);
                        }
                    case "AES-CTR":
                        return AesCtr(algorithm, key.Secret, data);
                    case "AES-GCM":
                        return AesGcmDecrypt(algorithm, key.Secret, data);
                    default:
                        using (var rsa = CreateRsa(key))
                        {
                            return rsa.Decrypt(data, GetOaepPadding(algorithm, key));
                        }
                }
            }
            catch (CryptographicException)
            {
                throw OperationError();
            }
        }

        private static Aes CreateAes(Byte[] key, CipherMode mode, PaddingMode padding)
        {
            var aes = Aes.Create();
            aes.Key = key;
            aes.Mode = mode;
            aes.Padding = padding;
            return aes;
        }

        private static Byte[] GetAesCbcIv(Algorithm algorithm)
        {
            var iv = algorithm.GetBytes("iv");
            return iv.Length == 16 ? iv : throw OperationError();
        }

        private static Byte[] AesCtr(Algorithm algorithm, Byte[] key, Byte[] data)
        {
            var counter = algorithm.GetBytes("counter");
            var length = algorithm.GetInteger("length") ?? throw TypeError("The length of the counter is missing.");

            if (counter.Length != 16 || length == 0 || length > 128)
            {
                throw OperationError();
            }

            var blocks = (data.Length + 15) / 16;

            //  the counter must not wrap around, as it would encrypt with the same stream again
            if (length < 63 && blocks > (1L << length))
            {
                throw OperationError();
            }

            var block = (Byte[])counter.Clone();
            var stream = new Byte[16];
            var result = new Byte[data.Length];

            using (var aes = CreateAes(key, CipherMode.ECB, PaddingMode.None))
            using (var encryptor = aes.CreateEncryptor())
            {
                for (var offset = 0; offset < data.Length; offset += 16)
                {
                    encryptor.TransformBlock(block, 0, 16, stream, 0);

                    for (var i = 0; i < 16 && offset + i < data.Length; i++)
                    {
                        result[offset + i] = (Byte)(data[offset + i] ^ stream[i]);
                    }

                    IncrementCounter(block, length);
                }
            }

            return result;
        }

        //  Only the rightmost bits (the given number) of the block are the counter.
        private static void IncrementCounter(Byte[] block, Int32 bits)
        {
            for (var i = block.Length - 1; i >= 0 && bits > 0; i--, bits -= 8)
            {
                var mask = bits >= 8 ? 0xFF : (1 << bits) - 1;
                var value = ((block[i] & mask) + 1) & mask;
                block[i] = (Byte)((block[i] & ~mask) | value);

                if (value != 0)
                {
                    break;
                }
            }
        }

        private static Byte[] AesGcmEncrypt(Algorithm algorithm, Byte[] key, Byte[] data)
        {
#if NET8_0_OR_GREATER
            var (iv, additionalData, tagLength) = GetAesGcmParameters(algorithm);
            var result = new Byte[data.Length + tagLength];

            using (var gcm = new AesGcm(key, tagLength))
            {
                gcm.Encrypt(iv, data, result.AsSpan(0, data.Length), result.AsSpan(data.Length), additionalData);
            }

            return result;
#else
            throw NotSupportedError();
#endif
        }

        private static Byte[] AesGcmDecrypt(Algorithm algorithm, Byte[] key, Byte[] data)
        {
#if NET8_0_OR_GREATER
            var (iv, additionalData, tagLength) = GetAesGcmParameters(algorithm);

            if (data.Length < tagLength)
            {
                throw OperationError();
            }

            var result = new Byte[data.Length - tagLength];

            using (var gcm = new AesGcm(key, tagLength))
            {
                gcm.Decrypt(iv, data.AsSpan(0, result.Length), data.AsSpan(result.Length), result, additionalData);
            }

            return result;
#else
            throw NotSupportedError();
#endif
        }

        private static (Byte[] Iv, Byte[] AdditionalData, Int32 TagLength) GetAesGcmParameters(Algorithm algorithm)
        {
            var iv = algorithm.GetBytes("iv");
            var additionalData = algorithm.GetBytes("additionalData", false);
            var tagLength = algorithm.GetInteger("tagLength") ?? 128;

            if (Array.IndexOf(new[] { 32, 64, 96, 104, 112, 120, 128 }, tagLength) < 0)
            {
                throw OperationError();
            }

            //  the platform takes 96 bit nonces and tags of at least 96 bits only
            if (iv.Length != 12 || tagLength < 96)
            {
                throw NotSupportedError();
            }

            return (iv, additionalData, tagLength / 8);
        }

        private static readonly Byte[] KeyWrapIv = { 0xA6, 0xA6, 0xA6, 0xA6, 0xA6, 0xA6, 0xA6, 0xA6 };

        //  RFC 3394
        private static Byte[] AesKeyWrap(Byte[] key, Byte[] data)
        {
            if (data.Length < 16 || data.Length % 8 != 0)
            {
                throw OperationError();
            }

            var n = data.Length / 8;
            var a = (Byte[])KeyWrapIv.Clone();
            var r = (Byte[])data.Clone();
            var block = new Byte[16];

            using (var aes = CreateAes(key, CipherMode.ECB, PaddingMode.None))
            using (var encryptor = aes.CreateEncryptor())
            {
                for (var j = 0; j < 6; j++)
                {
                    for (var i = 0; i < n; i++)
                    {
                        Buffer.BlockCopy(a, 0, block, 0, 8);
                        Buffer.BlockCopy(r, 8 * i, block, 8, 8);
                        encryptor.TransformBlock(block, 0, 16, block, 0);
                        Buffer.BlockCopy(block, 0, a, 0, 8);
                        XorCounter(a, (UInt64)(n * j + i + 1));
                        Buffer.BlockCopy(block, 8, r, 8 * i, 8);
                    }
                }
            }

            var result = new Byte[data.Length + 8];
            Buffer.BlockCopy(a, 0, result, 0, 8);
            Buffer.BlockCopy(r, 0, result, 8, data.Length);
            return result;
        }

        private static Byte[] AesKeyUnwrap(Byte[] key, Byte[] data)
        {
            if (data.Length < 24 || data.Length % 8 != 0)
            {
                throw OperationError();
            }

            var n = data.Length / 8 - 1;
            var a = new Byte[8];
            var r = new Byte[data.Length - 8];
            var block = new Byte[16];
            Buffer.BlockCopy(data, 0, a, 0, 8);
            Buffer.BlockCopy(data, 8, r, 0, r.Length);

            using (var aes = CreateAes(key, CipherMode.ECB, PaddingMode.None))
            using (var decryptor = aes.CreateDecryptor())
            {
                for (var j = 5; j >= 0; j--)
                {
                    for (var i = n - 1; i >= 0; i--)
                    {
                        XorCounter(a, (UInt64)(n * j + i + 1));
                        Buffer.BlockCopy(a, 0, block, 0, 8);
                        Buffer.BlockCopy(r, 8 * i, block, 8, 8);
                        decryptor.TransformBlock(block, 0, 16, block, 0);
                        Buffer.BlockCopy(block, 0, a, 0, 8);
                        Buffer.BlockCopy(block, 8, r, 8 * i, 8);
                    }
                }
            }

            return AreEqual(a, KeyWrapIv) ? r : throw OperationError();
        }

        private static void XorCounter(Byte[] a, UInt64 t)
        {
            for (var k = 7; k >= 0; k--, t >>= 8)
            {
                a[k] ^= (Byte)t;
            }
        }

        #endregion

        #region Derivation

        private static Byte[] DeriveBitsCore(Algorithm algorithm, CryptoKey key, Int32? length)
        {
            switch (algorithm.Name)
            {
                case "PBKDF2":
                {
                    var salt = algorithm.GetBytes("salt");
                    var iterations = algorithm.GetInteger("iterations") ?? throw TypeError("The iterations are missing.");
                    var hash = algorithm.GetHash();

                    if (length is null || length.Value % 8 != 0 || iterations == 0)
                    {
                        throw OperationError();
                    }

                    return Pbkdf2(hash, key.Secret, salt, iterations, length.Value / 8);
                }
                case "HKDF":
                {
                    var hash = algorithm.GetHash();
                    var salt = algorithm.GetBytes("salt");
                    var info = algorithm.GetBytes("info");

                    if (length is null || length.Value % 8 != 0 || length.Value / 8 > 255 * HashLength(hash))
                    {
                        throw OperationError();
                    }

                    return Hkdf(hash, key.Secret, salt, info, length.Value / 8);
                }
                default:
                {
                    var publicKey = algorithm.Get("public").FromJsValue() as CryptoKey ?? throw TypeError("The public key is missing.");

                    if (publicKey.Type != "public" || publicKey.AlgorithmName != "ECDH" || publicKey.Curve != key.Curve)
                    {
                        throw InvalidAccessError();
                    }

                    var secret = EcdhSecret(key, publicKey);

                    if (length is null)
                    {
                        return secret;
                    }
                    else if (length.Value > secret.Length * 8)
                    {
                        throw OperationError();
                    }

                    var result = new Byte[(length.Value + 7) / 8];
                    Buffer.BlockCopy(secret, 0, result, 0, result.Length);

                    if (length.Value % 8 != 0)
                    {
                        result[result.Length - 1] &= (Byte)(0xFF << (8 - length.Value % 8));
                    }

                    return result;
                }
            }
        }

        private static Byte[] EcdhSecret(CryptoKey privateKey, CryptoKey publicKey)
        {
#if NET8_0_OR_GREATER
            using (var mine = ECDiffieHellman.Create(privateKey.Ec.Value))
            using (var theirs = ECDiffieHellman.Create(publicKey.Ec.Value))
            {
                return mine.DeriveRawSecretAgreement(theirs.PublicKey);
            }
#else
            throw NotSupportedError();
#endif
        }

        private static Int32? GetKeyLength(Algorithm algorithm)
        {
            switch (algorithm.Name)
            {
                case "HMAC":
                {
                    var length = algorithm.GetInteger("length") ?? BlockBits(algorithm.GetHash());
                    return length != 0 ? length : throw TypeError("The length of the key can't be zero.");
                }
                case "PBKDF2":
                case "HKDF":
                    return null;
                default:
                {
                    var length = algorithm.GetInteger("length") ?? throw TypeError("The length of the key is missing.");
                    return length == 128 || length == 192 || length == 256 ? length : throw OperationError();
                }
            }
        }

        private static Byte[] Pbkdf2(String hash, Byte[] password, Byte[] salt, Int32 iterations, Int32 length)
        {
#if NET8_0_OR_GREATER
            return Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, HashName(hash), length);
#else
            var result = new Byte[length];
            var hashLength = HashLength(hash);

            using (var hmac = IncrementalHash.CreateHMAC(HashName(hash), password))
            {
                for (var block = 1; (block - 1) * hashLength < length; block++)
                {
                    hmac.AppendData(salt);
                    hmac.AppendData(new[] { (Byte)(block >> 24), (Byte)(block >> 16), (Byte)(block >> 8), (Byte)block });
                    var u = hmac.GetHashAndReset();
                    var t = (Byte[])u.Clone();

                    for (var i = 1; i < iterations; i++)
                    {
                        hmac.AppendData(u);
                        u = hmac.GetHashAndReset();

                        for (var k = 0; k < t.Length; k++)
                        {
                            t[k] ^= u[k];
                        }
                    }

                    var offset = (block - 1) * hashLength;
                    Buffer.BlockCopy(t, 0, result, offset, Math.Min(hashLength, length - offset));
                }
            }

            return result;
#endif
        }

        //  RFC 5869
        private static Byte[] Hkdf(String hash, Byte[] secret, Byte[] salt, Byte[] info, Int32 length)
        {
            var hashLength = HashLength(hash);
            var prk = Hmac(hash, salt.Length > 0 ? salt : new Byte[hashLength], secret);
            var result = new Byte[length];
            var previous = new Byte[0];

            for (var (i, offset) = (1, 0); offset < length; i++, offset += hashLength)
            {
                var input = new Byte[previous.Length + info.Length + 1];
                Buffer.BlockCopy(previous, 0, input, 0, previous.Length);
                Buffer.BlockCopy(info, 0, input, previous.Length, info.Length);
                input[input.Length - 1] = (Byte)i;
                previous = Hmac(hash, prk, input);
                Buffer.BlockCopy(previous, 0, result, offset, Math.Min(hashLength, length - offset));
            }

            return result;
        }

        #endregion

        #region Import

        private static CryptoKey ImportHmacKey(String format, Byte[] data, Jwk jwk, Algorithm algorithm, Boolean extractable, String[] usages)
        {
            CheckUsages(usages, Sign_, Verify_);
            var hash = algorithm.GetHash();

            if (jwk != null)
            {
                jwk.CheckType("oct");
                jwk.CheckUse("sig");
                jwk.CheckAlgorithm(hash == "SHA-1" ? "HS1" : "HS" + hash.Substring(4));
                data = jwk.GetBytes("k");
            }
            else if (format != "raw")
            {
                throw NotSupportedError();
            }

            var bits = data.Length * 8;
            var length = algorithm.GetInteger("length") ?? bits;

            if (bits == 0 || length > bits || length <= bits - 8)
            {
                throw DataError();
            }

            return CreateHmacKey(hash, data, length, extractable, usages);
        }

        private static CryptoKey ImportAesKey(String format, Byte[] data, Jwk jwk, String name, Boolean extractable, String[] usages)
        {
            CheckUsages(usages, GetAesUsages(name));

            if (jwk != null)
            {
                jwk.CheckType("oct");
                jwk.CheckUse("enc");
                data = jwk.GetBytes("k");
                jwk.CheckAlgorithm("A" + data.Length * 8 + name.Substring(4));
            }
            else if (format != "raw")
            {
                throw NotSupportedError();
            }

            if (data.Length != 16 && data.Length != 24 && data.Length != 32)
            {
                throw DataError();
            }

            return CreateAesKey(name, data, extractable, usages);
        }

        private static CryptoKey ImportEcKey(String format, Byte[] data, Jwk jwk, Algorithm algorithm, Boolean extractable, String[] usages)
        {
            var isEcdsa = algorithm.Name == "ECDSA";
            var curve = GetCurve(algorithm);
            var size = CoordinateLength(curve);
            ECParameters parameters;

            switch (format)
            {
                case "raw":
                    if (data.Length != 1 + 2 * size || data[0] != 4)
                    {
                        throw DataError();
                    }

                    parameters = new ECParameters
                    {
                        Curve = GetNamedCurve(curve),
                        Q = new ECPoint { X = Slice(data, 1, size), Y = Slice(data, 1 + size, size) },
                    };
                    break;
                case "jwk":
                    jwk.CheckType("EC");

                    if (jwk.Get("crv") != curve)
                    {
                        throw DataError();
                    }

                    if (isEcdsa)
                    {
                        jwk.CheckUse("sig");
                        jwk.CheckAlgorithm(curve == "P-521" ? "ES512" : "ES" + curve.Substring(2));
                    }

                    parameters = new ECParameters
                    {
                        Curve = GetNamedCurve(curve),
                        Q = new ECPoint { X = jwk.GetBytes("x", size), Y = jwk.GetBytes("y", size) },
                        D = jwk.Get("d") != null ? jwk.GetBytes("d", size) : null,
                    };
                    break;
                default:
                    parameters = ImportEcDer(format, data, curve);
                    break;
            }

            try
            {
                using (var ecdsa = ECDsa.Create(parameters))
                {
                    //  validated by the platform, e.g. that the point is on the curve
                }
            }
            catch (CryptographicException)
            {
                throw DataError();
            }

            var isPrivate = parameters.D != null;

            if (isPrivate)
            {
                CheckUsages(usages, isEcdsa ? new[] { Sign_ } : new[] { DeriveKey_, DeriveBits_ });
            }
            else
            {
                CheckUsages(usages, isEcdsa ? new[] { Verify_ } : new String[0]);
            }

            return CreateEcKey(algorithm.Name, curve, parameters, extractable, usages);
        }

        private static ECParameters ImportEcDer(String format, Byte[] data, String curve)
        {
#if NET8_0_OR_GREATER
            try
            {
                using (var ecdsa = ECDsa.Create())
                {
                    if (format == "spki")
                    {
                        ecdsa.ImportSubjectPublicKeyInfo(data, out _);
                    }
                    else
                    {
                        ecdsa.ImportPkcs8PrivateKey(data, out _);
                    }

                    var parameters = ecdsa.ExportParameters(format == "pkcs8");
                    return GetCurveName(parameters.Curve) == curve ? parameters : throw DataError();
                }
            }
            catch (CryptographicException)
            {
                throw DataError();
            }
#else
            throw NotSupportedError();
#endif
        }

        private static CryptoKey ImportRsaKey(String format, Byte[] data, Jwk jwk, Algorithm algorithm, Boolean extractable, String[] usages)
        {
            var hash = algorithm.GetHash();
            var (publicUsages, privateUsages) = GetRsaUsages(algorithm.Name);
            RSAParameters parameters;

            switch (format)
            {
                case "raw":
                    throw NotSupportedError();
                case "jwk":
                    jwk.CheckType("RSA");
                    jwk.CheckUse(algorithm.Name == "RSA-OAEP" ? "enc" : "sig");
                    jwk.CheckAlgorithm(GetRsaJwkAlgorithm(algorithm.Name, hash));
                    var modulus = TrimZeros(jwk.GetBytes("n"));
                    var half = (modulus.Length + 1) / 2;
                    parameters = new RSAParameters
                    {
                        Modulus = modulus,
                        Exponent = TrimZeros(jwk.GetBytes("e")),
                    };

                    if (jwk.Get("d") != null)
                    {
                        //  the platform needs the primes (and the values computed from them) too
                        if (jwk.Get("p") == null)
                        {
                            throw NotSupportedError();
                        }

                        parameters.D = jwk.GetBytes("d", modulus.Length);
                        parameters.P = jwk.GetBytes("p", half);
                        parameters.Q = jwk.GetBytes("q", half);
                        parameters.DP = jwk.GetBytes("dp", half);
                        parameters.DQ = jwk.GetBytes("dq", half);
                        parameters.InverseQ = jwk.GetBytes("qi", half);
                    }

                    break;
                default:
                    parameters = ImportRsaDer(format, data);
                    break;
            }

            try
            {
                using (var rsa = RSA.Create())
                {
                    rsa.ImportParameters(parameters);
                }
            }
            catch (CryptographicException)
            {
                throw DataError();
            }

            CheckUsages(usages, parameters.D != null ? privateUsages : publicUsages);
            return CreateRsaKey(algorithm.Name, hash, parameters, extractable, usages);
        }

        private static RSAParameters ImportRsaDer(String format, Byte[] data)
        {
#if NET8_0_OR_GREATER
            try
            {
                using (var rsa = RSA.Create())
                {
                    if (format == "spki")
                    {
                        rsa.ImportSubjectPublicKeyInfo(data, out _);
                    }
                    else
                    {
                        rsa.ImportPkcs8PrivateKey(data, out _);
                    }

                    return rsa.ExportParameters(format == "pkcs8");
                }
            }
            catch (CryptographicException)
            {
                throw DataError();
            }
#else
            throw NotSupportedError();
#endif
        }

        #endregion

        #region Export

        private static IDictionary<String, Object> ExportJwk(CryptoKey key)
        {
            var jwk = new Dictionary<String, Object>();

            switch (key.AlgorithmName)
            {
                case "HMAC":
                    jwk["kty"] = "oct";
                    jwk["k"] = ToBase64Url(key.Secret);
                    jwk["alg"] = key.Hash == "SHA-1" ? "HS1" : "HS" + key.Hash.Substring(4);
                    break;
                case "AES-CBC":
                case "AES-CTR":
                case "AES-GCM":
                case "AES-KW":
                    jwk["kty"] = "oct";
                    jwk["k"] = ToBase64Url(key.Secret);
                    jwk["alg"] = "A" + key.Secret.Length * 8 + key.AlgorithmName.Substring(4);
                    break;
                case "ECDSA":
                case "ECDH":
                {
                    var ec = key.Ec.Value;
                    var size = CoordinateLength(key.Curve);
                    jwk["kty"] = "EC";
                    jwk["crv"] = key.Curve;
                    jwk["x"] = ToBase64Url(Pad(ec.Q.X, size));
                    jwk["y"] = ToBase64Url(Pad(ec.Q.Y, size));

                    if (key.Type == "private")
                    {
                        jwk["d"] = ToBase64Url(Pad(ec.D, size));
                    }

                    break;
                }
                case "RSASSA-PKCS1-v1_5":
                case "RSA-PSS":
                case "RSA-OAEP":
                {
                    var rsa = key.Rsa.Value;
                    jwk["kty"] = "RSA";
                    jwk["alg"] = GetRsaJwkAlgorithm(key.AlgorithmName, key.Hash);
                    jwk["n"] = ToBase64Url(TrimZeros(rsa.Modulus));
                    jwk["e"] = ToBase64Url(TrimZeros(rsa.Exponent));

                    if (key.Type == "private")
                    {
                        jwk["d"] = ToBase64Url(TrimZeros(rsa.D));
                        jwk["p"] = ToBase64Url(TrimZeros(rsa.P));
                        jwk["q"] = ToBase64Url(TrimZeros(rsa.Q));
                        jwk["dp"] = ToBase64Url(TrimZeros(rsa.DP));
                        jwk["dq"] = ToBase64Url(TrimZeros(rsa.DQ));
                        jwk["qi"] = ToBase64Url(TrimZeros(rsa.InverseQ));
                    }

                    break;
                }
                default:
                    throw NotSupportedError();
            }

            jwk["key_ops"] = key.Usages;
            jwk["ext"] = key.Extractable;
            return jwk;
        }

        private static Byte[] ExportDer(String format, CryptoKey key)
        {
            var isPublic = format == "spki";

            if (key.Secret != null || (isPublic ? key.Type != "public" : key.Type != "private"))
            {
                throw InvalidAccessError();
            }

#if NET8_0_OR_GREATER
            if (key.Ec is ECParameters ec)
            {
                using (var ecdsa = ECDsa.Create(ec))
                {
                    return isPublic ? ecdsa.ExportSubjectPublicKeyInfo() : ecdsa.ExportPkcs8PrivateKey();
                }
            }

            using (var rsa = CreateRsa(key))
            {
                return isPublic ? rsa.ExportSubjectPublicKeyInfo() : rsa.ExportPkcs8PrivateKey();
            }
#else
            throw NotSupportedError();
#endif
        }

        private static String ToJson(Object value)
        {
            var engine = EngineInstance.Current;
            return new JsonSerializer(engine.Jint).Serialize(value.ToJsValue(engine)).ToString();
        }

        #endregion

        #region Keys

        private static CryptoKey CreateHmacKey(String hash, Byte[] data, Int32 length, Boolean extractable, String[] usages)
        {
            var algorithm = CreateAlgorithm("HMAC");
            algorithm["hash"] = CreateAlgorithm(hash);
            algorithm["length"] = length;
            return new CryptoKey("secret", extractable, algorithm, usages) { Secret = data, Hash = hash };
        }

        private static CryptoKey CreateAesKey(String name, Byte[] data, Boolean extractable, String[] usages)
        {
            var algorithm = CreateAlgorithm(name);
            algorithm["length"] = data.Length * 8;
            return new CryptoKey("secret", extractable, algorithm, usages) { Secret = data };
        }

        private static CryptoKey CreateEcKey(String name, String curve, ECParameters parameters, Boolean extractable, String[] usages)
        {
            var algorithm = CreateAlgorithm(name);
            algorithm["namedCurve"] = curve;
            var type = parameters.D != null ? "private" : "public";
            return new CryptoKey(type, extractable, algorithm, usages) { Ec = parameters, Curve = curve };
        }

        private static CryptoKey CreateRsaKey(String name, String hash, RSAParameters parameters, Boolean extractable, String[] usages)
        {
            var algorithm = CreateAlgorithm(name);
            algorithm["modulusLength"] = TrimZeros(parameters.Modulus).Length * 8;
            algorithm["publicExponent"] = new ArraySegment<Byte>(TrimZeros(parameters.Exponent));
            algorithm["hash"] = CreateAlgorithm(hash);
            var type = parameters.D != null ? "private" : "public";
            return new CryptoKey(type, extractable, algorithm, usages) { Rsa = parameters, Hash = hash };
        }

        private static IDictionary<String, Object> CreateKeyPair(CryptoKey publicKey, CryptoKey privateKey) =>
            new Dictionary<String, Object>
            {
                ["publicKey"] = publicKey,
                ["privateKey"] = privateKey,
            };

        private static Dictionary<String, Object> CreateAlgorithm(String name) =>
            new Dictionary<String, Object> { ["name"] = name };

        private static ECParameters PublicPart(ECParameters parameters) =>
            new ECParameters { Curve = parameters.Curve, Q = parameters.Q };

        private static RSAParameters PublicPart(RSAParameters parameters) =>
            new RSAParameters { Modulus = parameters.Modulus, Exponent = parameters.Exponent };

        private static RSA CreateRsa(CryptoKey key)
        {
            var rsa = RSA.Create();
            rsa.ImportParameters(key.Rsa.Value);
            return rsa;
        }

        private static RSASignaturePadding GetRsaPadding(Algorithm algorithm, CryptoKey key)
        {
            if (algorithm.Name == "RSA-PSS")
            {
                var saltLength = algorithm.GetInteger("saltLength") ?? throw TypeError("The saltLength is missing.");

                //  the platform uses a salt as long as the hash, which is what is used most
                return saltLength == HashLength(key.Hash) ? RSASignaturePadding.Pss : throw NotSupportedError();
            }

            return RSASignaturePadding.Pkcs1;
        }

        private static RSAEncryptionPadding GetOaepPadding(Algorithm algorithm, CryptoKey key)
        {
            var label = algorithm.GetBytes("label", false);

            if (label != null && label.Length > 0)
            {
                throw NotSupportedError();
            }

            switch (key.Hash)
            {
                case "SHA-1":
                    return RSAEncryptionPadding.OaepSHA1;
                case "SHA-256":
                    return RSAEncryptionPadding.OaepSHA256;
                case "SHA-384":
                    return RSAEncryptionPadding.OaepSHA384;
                default:
                    return RSAEncryptionPadding.OaepSHA512;
            }
        }

        private static String GetRsaJwkAlgorithm(String name, String hash)
        {
            var bits = hash == "SHA-1" ? "1" : hash.Substring(4);

            switch (name)
            {
                case "RSASSA-PKCS1-v1_5":
                    return "RS" + bits;
                case "RSA-PSS":
                    return "PS" + bits;
                default:
                    return hash == "SHA-1" ? "RSA-OAEP" : "RSA-OAEP-" + bits;
            }
        }

        private static (String[] Public, String[] Private) GetRsaUsages(String name) =>
            name == "RSA-OAEP"
                ? (new[] { Encrypt_, WrapKey_ }, new[] { Decrypt_, UnwrapKey_ })
                : (new[] { Verify_ }, new[] { Sign_ });

        private static String[] GetAesUsages(String name) =>
            name == "AES-KW" ? new[] { WrapKey_, UnwrapKey_ } : new[] { Encrypt_, Decrypt_, WrapKey_, UnwrapKey_ };

        private static String GetCurve(Algorithm algorithm)
        {
            var value = algorithm.Get("namedCurve");

            if (value.IsUndefined())
            {
                throw TypeError("The namedCurve is missing.");
            }

            var curve = TypeConverter.ToString(value);
            return curve == "P-256" || curve == "P-384" || curve == "P-521" ? curve : throw NotSupportedError();
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

        private static Int32 CoordinateLength(String curve) =>
            curve == "P-256" ? 32 : curve == "P-384" ? 48 : 66;

        private static Byte[] EncodePoint(ECParameters parameters, String curve)
        {
            var size = CoordinateLength(curve);
            var result = new Byte[1 + 2 * size];
            result[0] = 4;
            Buffer.BlockCopy(Pad(parameters.Q.X, size), 0, result, 1, size);
            Buffer.BlockCopy(Pad(parameters.Q.Y, size), 0, result, 1 + size, size);
            return result;
        }

        #endregion

        #region Checks

        private static Algorithm Normalize(Object algorithm, String operation)
        {
            String name;
            var source = algorithm as ObjectInstance;

            if (algorithm is String text)
            {
                name = text;
            }
            else if (source != null && !source.IsBufferSource())
            {
                var value = source.Get("name");

                if (value.IsUndefined())
                {
                    throw TypeError("The algorithm has no name.");
                }

                name = TypeConverter.ToString(value);
            }
            else
            {
                throw TypeError("The algorithm has to be a name or an object.");
            }

            var canonical = Operations.Keys.FirstOrDefault(key => key.Equals(name, StringComparison.OrdinalIgnoreCase));

            if (canonical is null || Array.IndexOf(Operations[canonical], operation) < 0)
            {
                throw NotSupportedError();
            }

            return new Algorithm(canonical, source);
        }

        private static Algorithm NormalizeWrap(Object algorithm, String operation, String fallback)
        {
            try
            {
                return Normalize(algorithm, operation);
            }
            catch (DomException)
            {
                return Normalize(algorithm, fallback);
            }
        }

        private static void CheckKey(CryptoKey key, Algorithm algorithm, String usage)
        {
            if (key is null)
            {
                throw TypeError("The key is missing.");
            }
            else if (key.AlgorithmName != algorithm.Name || !key.Allows(usage))
            {
                throw InvalidAccessError();
            }
        }

        private static void CheckFormat(String format)
        {
            if (format != "raw" && format != "spki" && format != "pkcs8" && format != "jwk")
            {
                throw TypeError($"The format '{format}' is not one of raw, spki, pkcs8 or jwk.");
            }
        }

        private static String[] CheckUsageValues(String[] usages)
        {
            usages = usages ?? throw TypeError("The key usages are missing.");

            foreach (var usage in usages)
            {
                if (Array.IndexOf(AllUsages, usage) < 0)
                {
                    throw TypeError($"The key usage '{usage}' is not valid.");
                }
            }

            return AllUsages.Where(usage => usages.Contains(usage)).ToArray();
        }

        private static void CheckUsages(String[] usages, params String[] allowed)
        {
            if (usages.Any(usage => Array.IndexOf(allowed, usage) < 0))
            {
                throw SyntaxError();
            }
        }

        private static String[] Intersect(String[] usages, String[] allowed) =>
            AllUsages.Where(usage => usages.Contains(usage) && allowed.Contains(usage)).ToArray();

        //  A secret or private key that can't be used for anything is an error.
        private static CryptoKey RequireUsages(CryptoKey key) =>
            key.Type != "public" && key.Usages.Length == 0 ? throw SyntaxError() : key;

        private static Byte[] Required(Byte[] data) =>
            data ?? throw TypeError("The data has to be an ArrayBuffer or a view on one.");

        #endregion

        #region Helpers

        private static HashAlgorithmName HashName(String hash)
        {
            switch (hash)
            {
                case "SHA-1":
                    return HashAlgorithmName.SHA1;
                case "SHA-256":
                    return HashAlgorithmName.SHA256;
                case "SHA-384":
                    return HashAlgorithmName.SHA384;
                default:
                    return HashAlgorithmName.SHA512;
            }
        }

        private static Int32 HashLength(String hash) =>
            hash == "SHA-1" ? 20 : hash == "SHA-256" ? 32 : hash == "SHA-384" ? 48 : 64;

        private static Int32 BlockBits(String hash) =>
            hash == "SHA-384" || hash == "SHA-512" ? 1024 : 512;

        private static Byte[] Hash(String hash, Byte[] data)
        {
            using (var algorithm = IncrementalHash.CreateHash(HashName(hash)))
            {
                algorithm.AppendData(data);
                return algorithm.GetHashAndReset();
            }
        }

        private static Byte[] Hmac(String hash, Byte[] key, Byte[] data)
        {
            using (var algorithm = IncrementalHash.CreateHMAC(HashName(hash), key))
            {
                algorithm.AppendData(data);
                return algorithm.GetHashAndReset();
            }
        }

        private static Byte[] RandomBytes(Int32 count)
        {
            var bytes = new Byte[count];

            using (var random = RandomNumberGenerator.Create())
            {
                random.GetBytes(bytes);
            }

            return bytes;
        }

        //  In constant time, so a signature can't be guessed byte by byte.
        private static Boolean AreEqual(Byte[] a, Byte[] b)
        {
            if (a.Length != b.Length)
            {
                return false;
            }

            var difference = 0;

            for (var i = 0; i < a.Length; i++)
            {
                difference |= a[i] ^ b[i];
            }

            return difference == 0;
        }

        private static Byte[] TrimZeros(Byte[] value)
        {
            var start = 0;

            while (start < value.Length - 1 && value[start] == 0)
            {
                start++;
            }

            return Slice(value, start, value.Length - start);
        }

        private static Byte[] Pad(Byte[] value, Int32 length)
        {
            value = TrimZeros(value);

            if (value.Length > length)
            {
                throw DataError();
            }

            var result = new Byte[length];
            Buffer.BlockCopy(value, 0, result, length - value.Length, value.Length);
            return result;
        }

        private static Byte[] Slice(Byte[] value, Int32 offset, Int32 length)
        {
            var result = new Byte[length];
            Buffer.BlockCopy(value, offset, result, 0, length);
            return result;
        }

        private static String ToBase64Url(Byte[] value) =>
            Convert.ToBase64String(value).TrimEnd('=').Replace('+', '-').Replace('/', '_');

        private static Byte[] FromBase64Url(String value)
        {
            var text = value.Replace('-', '+').Replace('_', '/');

            try
            {
                return Convert.FromBase64String(text.PadRight(text.Length + (4 - text.Length % 4) % 4, '='));
            }
            catch (FormatException)
            {
                throw DataError();
            }
        }

        private static Exception TypeError(String message) =>
            new JavaScriptException(EngineInstance.Current.Jint.Intrinsics.TypeError, message);

        private static Exception NotSupportedError() => new DomException(DomError.NotSupported);

        private static Exception SyntaxError() => new DomException(DomError.Syntax);

        private static Exception InvalidAccessError() => new DomException(DomError.InvalidAccess);

        private static Exception OperationError() => new DomException("OperationError");

        private static Exception DataError() => new DomException("DataError");

        #endregion

        #region Nested types

        /// <summary>
        /// A normalized algorithm: its name, and the object of a script with its parameters.
        /// </summary>
        private sealed class Algorithm
        {
            private readonly ObjectInstance _source;

            public Algorithm(String name, ObjectInstance source)
            {
                Name = name;
                _source = source;
            }

            public String Name { get; }

            public JsValue Get(String member) => _source?.Get(member) ?? JsValue.Undefined;

            public Byte[] GetBytes(String member, Boolean isRequired = true)
            {
                var value = Get(member);

                if (value.IsUndefined() && !isRequired)
                {
                    return null;
                }
                else if (value is ObjectInstance obj && value.IsBufferSource())
                {
                    return obj.GetBytes();
                }

                throw TypeError($"The {member} has to be an ArrayBuffer or a view on one.");
            }

            public Int32? GetInteger(String member)
            {
                var value = Get(member);
                return value.IsUndefined() ? (Int32?)null : (Int32)TypeConverter.ToUint32(value);
            }

            public String GetHash()
            {
                var value = Get("hash");

                if (value.IsUndefined())
                {
                    throw TypeError("The hash is missing.");
                }

                return Normalize(value.FromJsValue(), Digest_).Name;
            }
        }

        /// <summary>
        /// A JSON Web Key (RFC 7517) given by a script.
        /// </summary>
        private sealed class Jwk
        {
            private readonly ObjectInstance _source;

            public Jwk(ObjectInstance source)
            {
                _source = source;
            }

            public String Get(String member)
            {
                var value = _source.Get(member);
                return value.IsUndefined() || value.IsNull() ? null : TypeConverter.ToString(value);
            }

            public Byte[] GetBytes(String member) => FromBase64Url(Get(member) ?? throw DataError());

            public Byte[] GetBytes(String member, Int32 length) => Pad(GetBytes(member), length);

            public void Check(Boolean extractable, String[] usages)
            {
                if (Get("kty") is null)
                {
                    throw DataError();
                }

                var ext = _source.Get("ext");

                if (extractable && !ext.IsUndefined() && !TypeConverter.ToBoolean(ext))
                {
                    throw DataError();
                }

                if (_source.Get("key_ops") is JsArray ops)
                {
                    var allowed = Enumerable.Range(0, (Int32)TypeConverter.ToNumber(ops.Get("length")))
                        .Select(i => TypeConverter.ToString(ops.Get(i.ToString())))
                        .ToArray();

                    if (usages.Any(usage => !allowed.Contains(usage)))
                    {
                        throw DataError();
                    }
                }
            }

            public void CheckType(String kty)
            {
                if (Get("kty") != kty)
                {
                    throw DataError();
                }
            }

            public void CheckUse(String use)
            {
                var value = Get("use");

                if (value != null && value != use)
                {
                    throw DataError();
                }
            }

            public void CheckAlgorithm(String alg)
            {
                var value = Get("alg");

                if (value != null && value != alg)
                {
                    throw DataError();
                }
            }
        }

        #endregion
    }
}
