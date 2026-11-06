namespace AngleSharp.Js.Dom
{
    using AngleSharp.Attributes;
    using AngleSharp.Dom;
    using Jint;
    using Jint.Native;
    using Jint.Native.TypedArray;
    using System;
    using System.Security.Cryptography;

    /// <summary>
    /// Represents the Crypto interface: random numbers, without the operations of crypto.subtle.
    /// </summary>
    [DomName("Crypto")]
    public sealed class Crypto
    {
        private const Int32 MaxBytes = 65536;

        internal Crypto()
        {
        }

        /// <summary>
        /// Fills the given array of integers with random numbers.
        /// </summary>
        /// <param name="array">The array to fill.</param>
        /// <returns>The given array.</returns>
        [DomName("getRandomValues")]
        public JsTypedArray GetRandomValues(JsTypedArray array)
        {
            if (array is null || array.IsFloat16Array() || array.IsFloat32Array() || array.IsFloat64Array())
            {
                throw new DomException(DomError.TypeMismatch);
            }

            if (array.Get("byteLength").AsNumber() > MaxBytes)
            {
                throw new DomException(DomError.QuotaExceeded);
            }

            var bytes = new Byte[4];

            using (var random = RandomNumberGenerator.Create())
            {
                for (var i = 0; i < (Int32)array.Length; i++)
                {
                    random.GetBytes(bytes);
                    //  an integer array keeps what fits of the value, as for any assignment
                    array.Set(i, (Double)BitConverter.ToUInt32(bytes, 0));
                }
            }

            return array;
        }

        /// <summary>
        /// Creates a random UUID (version 4).
        /// </summary>
        /// <returns>The UUID in lowercase.</returns>
        [DomName("randomUUID")]
        public String RandomUuid() => Guid.NewGuid().ToString("D");
    }
}
