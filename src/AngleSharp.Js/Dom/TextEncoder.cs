namespace AngleSharp.Js.Dom
{
    using AngleSharp.Attributes;
    using AngleSharp.Js;
    using Jint;
    using Jint.Native;
    using Jint.Native.TypedArray;
    using System;
    using System.Text;

    /// <summary>
    /// Represents the TextEncoder of the Encoding standard, which turns text into UTF-8.
    /// </summary>
    [DomName("TextEncoder")]
    [DomExposed("Window")]
    [DomExposed("Worker")]
    public sealed class TextEncoder
    {
        //  What can't be encoded (a lone surrogate) becomes U+FFFD, as asked for by the standard.
        private static readonly Encoding Utf8 = new UTF8Encoding(false, false);

        /// <summary>
        /// Creates a new encoder.
        /// </summary>
        [DomConstructor]
        public TextEncoder()
        {
        }

        /// <summary>
        /// Gets the name of the encoding, which is always utf-8.
        /// </summary>
        [DomName("encoding")]
        public String Encoding => "utf-8";

        /// <summary>
        /// Encodes the text.
        /// </summary>
        /// <param name="input">The text to encode, if any.</param>
        /// <returns>The bytes of the text, which are a Uint8Array for scripts.</returns>
        [DomName("encode")]
        public ArraySegment<Byte> Encode(String input = null) =>
            new ArraySegment<Byte>(Utf8.GetBytes(input ?? String.Empty));

        /// <summary>
        /// Encodes as much of the text as fits into the given array, without splitting a character.
        /// </summary>
        /// <param name="source">The text to encode.</param>
        /// <param name="destination">The Uint8Array to write to.</param>
        /// <returns>An object telling how many code units were read and how many bytes written.</returns>
        [DomName("encodeInto")]
        public Object EncodeInto(String source, JsTypedArray destination)
        {
            var jint = EngineInstance.Current.Jint;
            var read = 0;
            var written = 0;

            if (destination is { } target && target.IsUint8Array())
            {
                var capacity = (Int32)target.Length;
                var text = source ?? String.Empty;

                while (read < text.Length)
                {
                    var length = Char.IsHighSurrogate(text[read]) && read + 1 < text.Length && Char.IsLowSurrogate(text[read + 1]) ? 2 : 1;
                    var bytes = Utf8.GetBytes(text.Substring(read, length));

                    if (written + bytes.Length > capacity)
                    {
                        break;
                    }

                    foreach (var value in bytes)
                    {
                        target.Set(written++, (Int32)value);
                    }

                    read += length;
                }
            }

            var result = new JsObject(jint);
            result.Set("read", read);
            result.Set("written", written);
            return result;
        }
    }

    /// <summary>
    /// Represents the options of a TextDecoder.
    /// </summary>
    [DomName("TextDecoderOptions")]
    public sealed class TextDecoderOptions
    {
        /// <summary>
        /// Gets or sets if invalid input is an error instead of becoming U+FFFD.
        /// </summary>
        [DomName("fatal")]
        public Boolean Fatal { get; set; }

        /// <summary>
        /// Gets or sets if a byte order mark stays in the text.
        /// </summary>
        [DomName("ignoreBOM")]
        public Boolean IgnoreBom { get; set; }
    }

    /// <summary>
    /// Represents the options of a call to decode.
    /// </summary>
    [DomName("TextDecodeOptions")]
    public sealed class TextDecodeOptions
    {
        /// <summary>
        /// Gets or sets if more input follows, so an incomplete character at the end has to wait.
        /// </summary>
        [DomName("stream")]
        public Boolean Stream { get; set; }
    }

    /// <summary>
    /// Represents the TextDecoder of the Encoding standard, which turns bytes into text.
    /// </summary>
    [DomName("TextDecoder")]
    [DomExposed("Window")]
    [DomExposed("Worker")]
    public sealed class TextDecoder
    {
        private readonly Encoding _encoding;
        private Decoder _decoder;
        private Boolean _isBomHandled;

        /// <summary>
        /// Creates a new decoder.
        /// </summary>
        /// <param name="label">The label of the encoding; by default utf-8.</param>
        /// <param name="options">The options, if any.</param>
        [DomConstructor]
        public TextDecoder(String label = null, TextDecoderOptions options = null)
        {
            var name = String.IsNullOrWhiteSpace(label) ? "utf-8" : label.Trim();

            if (!AngleSharp.Text.TextEncoding.IsSupported(name))
            {
                throw new Jint.Runtime.JavaScriptException(EngineInstance.Current.Jint.Construct("RangeError", $"The encoding '{label}' is not supported."));
            }

            var encoding = AngleSharp.Text.TextEncoding.Resolve(name);
            Fatal = options?.Fatal ?? false;
            IgnoreBom = options?.IgnoreBom ?? false;
            _encoding = Encoding.GetEncoding(encoding.WebName, new EncoderReplacementFallback(), Fatal ? DecoderFallback.ExceptionFallback : new DecoderReplacementFallback("�"));
        }

        /// <summary>
        /// Gets the name of the encoding.
        /// </summary>
        [DomName("encoding")]
        public String EncodingName => _encoding.WebName;

        /// <summary>
        /// Gets if invalid input is an error.
        /// </summary>
        [DomName("fatal")]
        public Boolean Fatal { get; }

        /// <summary>
        /// Gets if a byte order mark stays in the text.
        /// </summary>
        [DomName("ignoreBOM")]
        public Boolean IgnoreBom { get; }

        /// <summary>
        /// Decodes the given bytes.
        /// </summary>
        /// <param name="input">The bytes, if any: an ArrayBuffer or a view on one for scripts.</param>
        /// <param name="options">The options, if any.</param>
        /// <returns>The text.</returns>
        [DomName("decode")]
        public String Decode(Byte[] input = null, TextDecodeOptions options = null)
        {
            var isStreaming = options?.Stream ?? false;
            var bytes = input ?? new Byte[0];
            var decoder = _decoder ?? (_decoder = _encoding.GetDecoder());
            String text;

            try
            {
                var chars = new Char[_encoding.GetMaxCharCount(bytes.Length)];
                var count = decoder.GetChars(bytes, 0, bytes.Length, chars, 0, !isStreaming);
                text = new String(chars, 0, count);
            }
            catch (DecoderFallbackException)
            {
                _decoder = null;
                _isBomHandled = false;
                throw new Jint.Runtime.JavaScriptException(EngineInstance.Current.Jint.Intrinsics.TypeError, "The input is not valid.");
            }

            if (!_isBomHandled && text.Length > 0)
            {
                _isBomHandled = true;

                if (!IgnoreBom && text[0] == '﻿')
                {
                    text = text.Substring(1);
                }
            }

            if (!isStreaming)
            {
                _decoder = null;
                _isBomHandled = false;
            }

            return text;
        }
    }
}
