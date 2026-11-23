#if !NET8_0_OR_GREATER
namespace AngleSharp.Js.Dom
{
    using System;
    using System.Collections.Generic;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// The formats spki (SubjectPublicKeyInfo, RFC 5280) and pkcs8 (PrivateKeyInfo, RFC 5208)
    /// of the EC (RFC 5480, RFC 5915) and RSA (RFC 8017) keys, in DER. .NET brings them only
    /// since .NET Core 3, so the older targets get them from here. Malformed data throws a
    /// CryptographicException.
    /// </summary>
    static class KeyDer
    {
        private const String EcPublicKeyOid = "1.2.840.10045.2.1";
        private const String RsaEncryptionOid = "1.2.840.113549.1.1.1";

        private const Byte IntegerTag = 0x02;
        private const Byte BitStringTag = 0x03;
        private const Byte OctetStringTag = 0x04;
        private const Byte NullTag = 0x05;
        private const Byte OidTag = 0x06;
        private const Byte SequenceTag = 0x30;
        private const Byte EcParametersTag = 0xA0;
        private const Byte EcPublicKeyTag = 0xA1;

        #region EC

        /// <summary>
        /// Reads an EC key on the given curve. The public point of a pkcs8 key is optional: the
        /// coordinates are null without it.
        /// </summary>
        public static EcKeyParameters ReadEc(String format, Byte[] data, String curve)
        {
            var info = ReadInfo(format, data);
            var algorithm = info.ReadSequence();

            if (algorithm.ReadOid() != EcPublicKeyOid || GetCurveName(algorithm.ReadOid()) != curve)
            {
                throw Malformed();
            }

            algorithm.End();

            if (format == "spki")
            {
                var point = info.ReadBitString();
                info.End();
                return FromPoint(curve, point, null);
            }

            var key = ReadSingle(info.ReadOctetString());
            Byte[] publicPoint = null;

            if (!IsInteger(key.ReadInteger(), 1))
            {
                throw Malformed();
            }

            var d = key.ReadOctetString();

            while (key.HasMore)
            {
                var tag = key.PeekTag();

                if (tag == EcParametersTag)
                {
                    var parameters = key.Read(EcParametersTag);

                    if (GetCurveName(parameters.ReadOid()) != curve)
                    {
                        throw Malformed();
                    }
                }
                else if (tag == EcPublicKeyTag)
                {
                    publicPoint = key.Read(EcPublicKeyTag).ReadBitString();
                }
                else
                {
                    throw Malformed();
                }
            }

            return publicPoint != null ? FromPoint(curve, publicPoint, d) : new EcKeyParameters(curve, null, null, Pad(d, EcKeyParameters.CoordinateLength(curve)));
        }

        public static Byte[] WriteEc(String format, EcKeyParameters key)
        {
            var size = EcKeyParameters.CoordinateLength(key.Curve);
            var point = new Byte[1 + 2 * size];
            point[0] = 4;
            Buffer.BlockCopy(Pad(key.X, size), 0, point, 1, size);
            Buffer.BlockCopy(Pad(key.Y, size), 0, point, 1 + size, size);
            var algorithm = Encode(SequenceTag, EncodeOid(EcPublicKeyOid), EncodeOid(GetCurveOid(key.Curve)));

            if (format == "spki")
            {
                return Encode(SequenceTag, algorithm, EncodeBitString(point));
            }

            var privateKey = Encode(SequenceTag,
                EncodeInteger(1),
                Encode(OctetStringTag, Pad(key.D, size)),
                Encode(EcPublicKeyTag, EncodeBitString(point)));
            return Encode(SequenceTag, EncodeInteger(0), algorithm, Encode(OctetStringTag, privateKey));
        }

        private static EcKeyParameters FromPoint(String curve, Byte[] point, Byte[] d)
        {
            var size = EcKeyParameters.CoordinateLength(curve);

            //  only the uncompressed form of the point
            if (point.Length != 1 + 2 * size || point[0] != 4)
            {
                throw Malformed();
            }

            var x = new Byte[size];
            var y = new Byte[size];
            Buffer.BlockCopy(point, 1, x, 0, size);
            Buffer.BlockCopy(point, 1 + size, y, 0, size);
            return new EcKeyParameters(curve, x, y, d != null ? Pad(d, size) : null);
        }

        private static String GetCurveName(String oid)
        {
            switch (oid)
            {
                case "1.2.840.10045.3.1.7":
                    return "P-256";
                case "1.3.132.0.34":
                    return "P-384";
                case "1.3.132.0.35":
                    return "P-521";
                default:
                    return null;
            }
        }

        private static String GetCurveOid(String curve)
        {
            switch (curve)
            {
                case "P-256":
                    return "1.2.840.10045.3.1.7";
                case "P-384":
                    return "1.3.132.0.34";
                default:
                    return "1.3.132.0.35";
            }
        }

        #endregion

        #region RSA

        public static RSAParameters ReadRsa(String format, Byte[] data)
        {
            var info = ReadInfo(format, data);
            var algorithm = info.ReadSequence();

            if (algorithm.ReadOid() != RsaEncryptionOid)
            {
                throw Malformed();
            }

            //  the parameters are NULL, which some encoders leave out
            if (algorithm.HasMore)
            {
                algorithm.ReadNull();
            }

            algorithm.End();

            if (format == "spki")
            {
                var publicKey = ReadSingle(info.ReadBitString());
                info.End();
                var parameters = new RSAParameters
                {
                    Modulus = publicKey.ReadInteger(),
                    Exponent = publicKey.ReadInteger(),
                };
                publicKey.End();
                return parameters;
            }

            var key = ReadSingle(info.ReadOctetString());

            //  version 0: two primes, which is all the platform takes
            if (!IsInteger(key.ReadInteger(), 0))
            {
                throw Malformed();
            }

            var modulus = key.ReadInteger();
            var half = (modulus.Length + 1) / 2;
            var result = new RSAParameters
            {
                Modulus = modulus,
                Exponent = key.ReadInteger(),
                D = Pad(key.ReadInteger(), modulus.Length),
                P = Pad(key.ReadInteger(), half),
                Q = Pad(key.ReadInteger(), half),
                DP = Pad(key.ReadInteger(), half),
                DQ = Pad(key.ReadInteger(), half),
                InverseQ = Pad(key.ReadInteger(), half),
            };
            key.End();
            return result;
        }

        public static Byte[] WriteRsa(String format, RSAParameters key)
        {
            var algorithm = Encode(SequenceTag, EncodeOid(RsaEncryptionOid), Encode(NullTag));

            if (format == "spki")
            {
                var publicKey = Encode(SequenceTag, EncodeInteger(key.Modulus), EncodeInteger(key.Exponent));
                return Encode(SequenceTag, algorithm, EncodeBitString(publicKey));
            }

            var privateKey = Encode(SequenceTag,
                EncodeInteger(0),
                EncodeInteger(key.Modulus),
                EncodeInteger(key.Exponent),
                EncodeInteger(key.D),
                EncodeInteger(key.P),
                EncodeInteger(key.Q),
                EncodeInteger(key.DP),
                EncodeInteger(key.DQ),
                EncodeInteger(key.InverseQ));
            return Encode(SequenceTag, EncodeInteger(0), algorithm, Encode(OctetStringTag, privateKey));
        }

        #endregion

        #region Helpers

        //  The outer SEQUENCE of either format, past the version of a PrivateKeyInfo.
        private static DerReader ReadInfo(String format, Byte[] data)
        {
            var info = ReadSingle(data);

            //  version 1 is a OneAsymmetricKey (RFC 5958), which may carry the public key too
            if (format == "pkcs8")
            {
                var version = info.ReadInteger();

                if (!IsInteger(version, 0) && !IsInteger(version, 1))
                {
                    throw Malformed();
                }
            }

            return info;
        }

        //  The one SEQUENCE the data consists of.
        private static DerReader ReadSingle(Byte[] data)
        {
            var reader = new DerReader(data, 0, data.Length);
            var sequence = reader.ReadSequence();
            reader.End();
            return sequence;
        }

        private static Boolean IsInteger(Byte[] value, Byte expected) => value.Length == 1 && value[0] == expected;

        private static Byte[] Pad(Byte[] value, Int32 length)
        {
            var start = 0;

            while (value.Length - start > length && value[start] == 0)
            {
                start++;
            }

            if (value.Length - start > length)
            {
                throw Malformed();
            }

            var result = new Byte[length];
            Buffer.BlockCopy(value, start, result, length - (value.Length - start), value.Length - start);
            return result;
        }

        private static Byte[] Encode(Byte tag, params Byte[][] contents)
        {
            var length = 0;

            for (var i = 0; i < contents.Length; i++)
            {
                length += contents[i].Length;
            }

            var lengthBytes = length < 0x80 ? 0 : length <= 0xFF ? 1 : length <= 0xFFFF ? 2 : 3;
            var result = new Byte[2 + lengthBytes + length];
            var offset = 2 + lengthBytes;
            result[0] = tag;

            if (lengthBytes == 0)
            {
                result[1] = (Byte)length;
            }
            else
            {
                result[1] = (Byte)(0x80 | lengthBytes);

                for (var i = 0; i < lengthBytes; i++)
                {
                    result[1 + lengthBytes - i] = (Byte)(length >> (8 * i));
                }
            }

            for (var i = 0; i < contents.Length; i++)
            {
                Buffer.BlockCopy(contents[i], 0, result, offset, contents[i].Length);
                offset += contents[i].Length;
            }

            return result;
        }

        private static Byte[] EncodeInteger(Int32 value) => Encode(IntegerTag, new[] { (Byte)value });

        //  The (unsigned, big endian) value as the shortest positive INTEGER.
        private static Byte[] EncodeInteger(Byte[] value)
        {
            var start = 0;

            while (start < value.Length - 1 && value[start] == 0)
            {
                start++;
            }

            var sign = (value[start] & 0x80) != 0 ? 1 : 0;
            var content = new Byte[sign + value.Length - start];
            Buffer.BlockCopy(value, start, content, sign, value.Length - start);
            return Encode(IntegerTag, content);
        }

        private static Byte[] EncodeBitString(Byte[] value)
        {
            var content = new Byte[1 + value.Length];
            Buffer.BlockCopy(value, 0, content, 1, value.Length);
            return Encode(BitStringTag, content);
        }

        private static Byte[] EncodeOid(String oid)
        {
            var arcs = oid.Split('.');
            var content = new List<Byte>();
            AppendArc(content, 40 * Int64.Parse(arcs[0]) + Int64.Parse(arcs[1]));

            for (var i = 2; i < arcs.Length; i++)
            {
                AppendArc(content, Int64.Parse(arcs[i]));
            }

            return Encode(OidTag, content.ToArray());
        }

        //  Base 128, most significant group first, all but the last with the high bit set.
        private static void AppendArc(List<Byte> content, Int64 arc)
        {
            var start = content.Count;
            content.Add((Byte)(arc & 0x7F));

            while ((arc >>= 7) > 0)
            {
                content.Insert(start, (Byte)(0x80 | (arc & 0x7F)));
            }
        }

        private static Exception Malformed() => new CryptographicException("The key data is not valid DER.");

        #endregion

        #region Reader

        /// <summary>
        /// Reads the elements of a DER encoded constructed value one after another.
        /// </summary>
        private sealed class DerReader
        {
            private readonly Byte[] _data;
            private readonly Int32 _end;
            private Int32 _position;

            public DerReader(Byte[] data, Int32 offset, Int32 end)
            {
                _data = data;
                _position = offset;
                _end = end;
            }

            public Boolean HasMore => _position < _end;

            public Byte PeekTag() => HasMore ? _data[_position] : throw Malformed();

            public DerReader ReadSequence() => Read(SequenceTag);

            //  The contents of the next element, which has to have the given tag.
            public DerReader Read(Byte tag)
            {
                var length = ReadHeader(tag);
                var reader = new DerReader(_data, _position, _position + length);
                _position += length;
                return reader;
            }

            //  The magnitude of a non-negative INTEGER, big endian, without leading zeros.
            public Byte[] ReadInteger()
            {
                var value = ReadBytes(IntegerTag);

                if (value.Length == 0 || (value[0] & 0x80) != 0)
                {
                    throw Malformed();
                }

                var start = 0;

                while (start < value.Length - 1 && value[start] == 0)
                {
                    start++;
                }

                return start == 0 ? value : Slice(value, start, value.Length - start);
            }

            public Byte[] ReadOctetString() => ReadBytes(OctetStringTag);

            public Byte[] ReadBitString()
            {
                var value = ReadBytes(BitStringTag);

                //  the keys are whole bytes, so there can't be unused bits
                if (value.Length == 0 || value[0] != 0)
                {
                    throw Malformed();
                }

                return Slice(value, 1, value.Length - 1);
            }

            public void ReadNull()
            {
                if (ReadHeader(NullTag) != 0)
                {
                    throw Malformed();
                }
            }

            public String ReadOid()
            {
                var value = ReadBytes(OidTag);
                var result = new StringBuilder();
                var arc = 0L;

                for (var i = 0; i < value.Length; i++)
                {
                    if (arc > (Int64.MaxValue >> 7))
                    {
                        throw Malformed();
                    }

                    arc = (arc << 7) | (value[i] & 0x7Fu);

                    if ((value[i] & 0x80) == 0)
                    {
                        if (result.Length == 0)
                        {
                            var first = Math.Min(arc / 40, 2);
                            result.Append(first).Append('.').Append(arc - 40 * first);
                        }
                        else
                        {
                            result.Append('.').Append(arc);
                        }

                        arc = 0;
                    }
                }

                return result.Length > 0 && (value[value.Length - 1] & 0x80) == 0 ? result.ToString() : throw Malformed();
            }

            public void End()
            {
                if (HasMore)
                {
                    throw Malformed();
                }
            }

            private Byte[] ReadBytes(Byte tag)
            {
                var length = ReadHeader(tag);
                var value = Slice(_data, _position, length);
                _position += length;
                return value;
            }

            //  The length of the element with the given tag; the position is then at its contents.
            private Int32 ReadHeader(Byte tag)
            {
                if (_end - _position < 2 || _data[_position] != tag)
                {
                    throw Malformed();
                }

                var first = _data[_position + 1];
                var length = 0;
                _position += 2;

                if (first < 0x80)
                {
                    length = first;
                }
                else
                {
                    //  the definite long form only, of at most three bytes (16 MB)
                    var count = first & 0x7F;

                    if (count == 0 || count > 3 || _end - _position < count)
                    {
                        throw Malformed();
                    }

                    for (var i = 0; i < count; i++)
                    {
                        length = (length << 8) | _data[_position++];
                    }
                }

                return length <= _end - _position ? length : throw Malformed();
            }

            private static Byte[] Slice(Byte[] value, Int32 offset, Int32 length)
            {
                var result = new Byte[length];
                Buffer.BlockCopy(value, offset, result, 0, length);
                return result;
            }
        }

        #endregion
    }
}
#endif
