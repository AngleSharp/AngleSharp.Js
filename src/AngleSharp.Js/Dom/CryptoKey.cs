namespace AngleSharp.Js.Dom
{
    using AngleSharp.Attributes;
    using System;
    using System.Collections.Generic;
    using System.Security.Cryptography;

    /// <summary>
    /// Represents a key of the Web Cryptography API, which scripts get from crypto.subtle
    /// without being able to see its material unless they export an extractable one.
    /// </summary>
    [DomName("CryptoKey")]
    [DomExposed("Window")]
    [DomExposed("Worker")]
    public sealed class CryptoKey
    {
        private readonly IDictionary<String, Object> _algorithm;
        private readonly String[] _usages;

        internal CryptoKey(String type, Boolean extractable, IDictionary<String, Object> algorithm, String[] usages)
        {
            Type = type;
            Extractable = extractable;
            _algorithm = algorithm;
            _usages = usages;
        }

        /// <summary>
        /// Gets the type of the key: "secret", "public" or "private".
        /// </summary>
        [DomName("type")]
        public String Type { get; }

        /// <summary>
        /// Gets if the key may be exported.
        /// </summary>
        [DomName("extractable")]
        public Boolean Extractable { get; }

        /// <summary>
        /// Gets the algorithm the key is for and its parameters (e.g. the name, the length and
        /// the hash), which is a plain object for scripts.
        /// </summary>
        [DomName("algorithm")]
        public IDictionary<String, Object> Algorithm => new Dictionary<String, Object>(_algorithm);

        /// <summary>
        /// Gets what the key may be used for, e.g. "encrypt" or "sign".
        /// </summary>
        [DomName("usages")]
        public String[] Usages => (String[])_usages.Clone();

        internal String AlgorithmName => (String)_algorithm["name"];

        internal String Hash { get; set; }

        internal String Curve { get; set; }

        internal Byte[] Secret { get; set; }

        internal RSAParameters? Rsa { get; set; }

        internal ECParameters? Ec { get; set; }

        internal Boolean Allows(String usage) => Array.IndexOf(_usages, usage) >= 0;
    }
}
