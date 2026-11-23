namespace AngleSharp.Js.Dom
{
    using AngleSharp.Attributes;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents the SubtleCrypto interface of the Web Cryptography API (crypto.subtle): hashes
    /// (SHA-1, SHA-256, SHA-384, SHA-512), HMAC, AES (CBC, CTR, GCM, KW), PBKDF2, HKDF, ECDSA,
    /// ECDH (P-256, P-384, P-521), RSASSA-PKCS1-v1_5, RSA-PSS and RSA-OAEP. Every operation
    /// gives a promise, which is rejected (rather than throwing) if the operation fails.
    /// </summary>
    [DomName("SubtleCrypto")]
    [DomExposed("Window")]
    [DomExposed("Worker")]
    public sealed class SubtleCrypto
    {
        internal SubtleCrypto()
        {
        }

        /// <summary>
        /// Encrypts the data.
        /// </summary>
        /// <param name="algorithm">The name of the algorithm or an object with its parameters.</param>
        /// <param name="key">The key to encrypt with.</param>
        /// <param name="data">The data to encrypt.</param>
        /// <returns>The encrypted data, an ArrayBuffer for scripts.</returns>
        [DomName("encrypt")]
        public Task<Byte[]> Encrypt(Object algorithm, CryptoKey key, Byte[] data) =>
            Run(() => WebCrypto.Encrypt(algorithm, key, data));

        /// <summary>
        /// Decrypts the data.
        /// </summary>
        /// <param name="algorithm">The name of the algorithm or an object with its parameters.</param>
        /// <param name="key">The key to decrypt with.</param>
        /// <param name="data">The data to decrypt.</param>
        /// <returns>The decrypted data, an ArrayBuffer for scripts.</returns>
        [DomName("decrypt")]
        public Task<Byte[]> Decrypt(Object algorithm, CryptoKey key, Byte[] data) =>
            Run(() => WebCrypto.Decrypt(algorithm, key, data));

        /// <summary>
        /// Signs the data.
        /// </summary>
        /// <param name="algorithm">The name of the algorithm or an object with its parameters.</param>
        /// <param name="key">The key to sign with.</param>
        /// <param name="data">The data to sign.</param>
        /// <returns>The signature, an ArrayBuffer for scripts.</returns>
        [DomName("sign")]
        public Task<Byte[]> Sign(Object algorithm, CryptoKey key, Byte[] data) =>
            Run(() => WebCrypto.Sign(algorithm, key, data));

        /// <summary>
        /// Verifies the signature of the data.
        /// </summary>
        /// <param name="algorithm">The name of the algorithm or an object with its parameters.</param>
        /// <param name="key">The key to verify with.</param>
        /// <param name="signature">The signature.</param>
        /// <param name="data">The data that was signed.</param>
        /// <returns>True if the signature is valid.</returns>
        [DomName("verify")]
        public Task<Boolean> Verify(Object algorithm, CryptoKey key, Byte[] signature, Byte[] data) =>
            Run(() => WebCrypto.Verify(algorithm, key, signature, data));

        /// <summary>
        /// Computes the digest (hash) of the data.
        /// </summary>
        /// <param name="algorithm">The name of the hash, e.g. "SHA-256".</param>
        /// <param name="data">The data to hash.</param>
        /// <returns>The digest, an ArrayBuffer for scripts.</returns>
        [DomName("digest")]
        public Task<Byte[]> Digest(Object algorithm, Byte[] data) =>
            Run(() => WebCrypto.Digest(algorithm, data));

        /// <summary>
        /// Generates a key, or a key pair ({ publicKey, privateKey }) for the asymmetric algorithms.
        /// </summary>
        /// <param name="algorithm">The algorithm and its parameters.</param>
        /// <param name="extractable">If the (private) key may be exported.</param>
        /// <param name="keyUsages">What the key may be used for.</param>
        /// <returns>The key or the key pair.</returns>
        [DomName("generateKey")]
        public Task<Object> GenerateKey(Object algorithm, Boolean extractable, String[] keyUsages) =>
            Run(() => WebCrypto.GenerateKey(algorithm, extractable, keyUsages));

        /// <summary>
        /// Derives a key from a base key.
        /// </summary>
        /// <param name="algorithm">The algorithm of the derivation and its parameters.</param>
        /// <param name="baseKey">The key to derive from.</param>
        /// <param name="derivedKeyType">The algorithm of the derived key and its parameters.</param>
        /// <param name="extractable">If the derived key may be exported.</param>
        /// <param name="keyUsages">What the derived key may be used for.</param>
        /// <returns>The derived key.</returns>
        [DomName("deriveKey")]
        public Task<CryptoKey> DeriveKey(Object algorithm, CryptoKey baseKey, Object derivedKeyType, Boolean extractable, String[] keyUsages) =>
            Run(() => WebCrypto.DeriveKey(algorithm, baseKey, derivedKeyType, extractable, keyUsages));

        /// <summary>
        /// Derives bits from a base key.
        /// </summary>
        /// <param name="algorithm">The algorithm of the derivation and its parameters.</param>
        /// <param name="baseKey">The key to derive from.</param>
        /// <param name="length">The number of bits to derive, null for all (ECDH only).</param>
        /// <returns>The derived bits, an ArrayBuffer for scripts.</returns>
        [DomName("deriveBits")]
        public Task<Byte[]> DeriveBits(Object algorithm, CryptoKey baseKey, Int32? length = null) =>
            Run(() => WebCrypto.DeriveBits(algorithm, baseKey, length));

        /// <summary>
        /// Imports a key.
        /// </summary>
        /// <param name="format">The format of the data: "raw", "spki", "pkcs8" or "jwk".</param>
        /// <param name="keyData">The key, bytes or a JSON Web Key object.</param>
        /// <param name="algorithm">The algorithm the key is for and its parameters.</param>
        /// <param name="extractable">If the key may be exported.</param>
        /// <param name="keyUsages">What the key may be used for.</param>
        /// <returns>The imported key.</returns>
        [DomName("importKey")]
        public Task<CryptoKey> ImportKey(String format, Object keyData, Object algorithm, Boolean extractable, String[] keyUsages) =>
            Run(() => WebCrypto.ImportKey(format, keyData, algorithm, extractable, keyUsages));

        /// <summary>
        /// Exports an extractable key.
        /// </summary>
        /// <param name="format">The format: "raw", "spki", "pkcs8" or "jwk".</param>
        /// <param name="key">The key to export.</param>
        /// <returns>An ArrayBuffer, or a JSON Web Key object for "jwk".</returns>
        [DomName("exportKey")]
        public Task<Object> ExportKey(String format, CryptoKey key) =>
            Run(() => WebCrypto.ExportKey(format, key));

        /// <summary>
        /// Exports a key and encrypts it with another one.
        /// </summary>
        /// <param name="format">The format to export the key in.</param>
        /// <param name="key">The key to wrap.</param>
        /// <param name="wrappingKey">The key to encrypt with.</param>
        /// <param name="wrapAlgorithm">The algorithm to encrypt with and its parameters.</param>
        /// <returns>The wrapped key, an ArrayBuffer for scripts.</returns>
        [DomName("wrapKey")]
        public Task<Byte[]> WrapKey(String format, CryptoKey key, CryptoKey wrappingKey, Object wrapAlgorithm) =>
            Run(() => WebCrypto.WrapKey(format, key, wrappingKey, wrapAlgorithm));

        /// <summary>
        /// Decrypts a wrapped key and imports it.
        /// </summary>
        /// <param name="format">The format the key was exported in.</param>
        /// <param name="wrappedKey">The wrapped key.</param>
        /// <param name="unwrappingKey">The key to decrypt with.</param>
        /// <param name="unwrapAlgorithm">The algorithm to decrypt with and its parameters.</param>
        /// <param name="unwrappedKeyAlgorithm">The algorithm the unwrapped key is for.</param>
        /// <param name="extractable">If the unwrapped key may be exported.</param>
        /// <param name="keyUsages">What the unwrapped key may be used for.</param>
        /// <returns>The unwrapped key.</returns>
        [DomName("unwrapKey")]
        public Task<CryptoKey> UnwrapKey(String format, Byte[] wrappedKey, CryptoKey unwrappingKey, Object unwrapAlgorithm, Object unwrappedKeyAlgorithm, Boolean extractable, String[] keyUsages) =>
            Run(() => WebCrypto.UnwrapKey(format, wrappedKey, unwrappingKey, unwrapAlgorithm, unwrappedKeyAlgorithm, extractable, keyUsages));

        //  The work is done right away (it is quick enough), but a failure has to reject the
        //  promise rather than being thrown, as the standard asks for.
        private static Task<T> Run<T>(Func<T> operation)
        {
            try
            {
                return Task.FromResult(operation());
            }
            catch (Exception ex)
            {
                return Task.FromException<T>(ex);
            }
        }
    }
}
