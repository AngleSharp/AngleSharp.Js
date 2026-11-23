#if !NET8_0_OR_GREATER
namespace AngleSharp.Js.Dom
{
    using System;
    using System.Security.Cryptography;

    /// <summary>
    /// AES-GCM (NIST SP 800-38D) on top of the AES block cipher, for the targets without
    /// AesGcm. Only for the 96 bit IVs WebCrypto lets through, which make the first counter
    /// block the IV followed by 1. A tag that does not match throws a CryptographicException.
    /// </summary>
    static class ManagedAesGcm
    {
        private const Int32 BlockSize = 16;

        //  The reduction polynomial of GF(2^128) in the bit order of GCM.
        private const UInt64 R = 0xE100000000000000;

        public static Byte[] Encrypt(Byte[] key, Byte[] iv, Byte[] data, Byte[] additionalData, Int32 tagLength)
        {
            var result = new Byte[data.Length + tagLength];

            using (var aes = CreateAes(key))
            using (var encryptor = aes.CreateEncryptor())
            {
                var j0 = GetFirstCounter(iv);
                Ctr(encryptor, j0, data, data.Length, result);
                var tag = ComputeTag(encryptor, j0, additionalData ?? new Byte[0], result, data.Length);
                Buffer.BlockCopy(tag, 0, result, data.Length, tagLength);
            }

            return result;
        }

        public static Byte[] Decrypt(Byte[] key, Byte[] iv, Byte[] data, Byte[] additionalData, Int32 tagLength)
        {
            var length = data.Length - tagLength;
            var result = new Byte[length];

            using (var aes = CreateAes(key))
            using (var encryptor = aes.CreateEncryptor())
            {
                var j0 = GetFirstCounter(iv);
                var tag = ComputeTag(encryptor, j0, additionalData ?? new Byte[0], data, length);
                var difference = 0;

                //  in constant time, so a tag can't be guessed byte by byte
                for (var i = 0; i < tagLength; i++)
                {
                    difference |= tag[i] ^ data[length + i];
                }

                if (difference != 0)
                {
                    throw new CryptographicException("The tag of the data does not match.");
                }

                Ctr(encryptor, j0, data, length, result);
            }

            return result;
        }

        private static Aes CreateAes(Byte[] key)
        {
            var aes = Aes.Create();
            aes.Key = key;
            aes.Mode = CipherMode.ECB;
            aes.Padding = PaddingMode.None;
            return aes;
        }

        private static Byte[] GetFirstCounter(Byte[] iv)
        {
            var j0 = new Byte[BlockSize];
            Buffer.BlockCopy(iv, 0, j0, 0, 12);
            j0[BlockSize - 1] = 1;
            return j0;
        }

        //  GCTR, starting with the block after the first counter block (which is for the tag).
        private static void Ctr(ICryptoTransform encryptor, Byte[] j0, Byte[] input, Int32 length, Byte[] output)
        {
            var counter = (Byte[])j0.Clone();
            var stream = new Byte[BlockSize];

            for (var offset = 0; offset < length; offset += BlockSize)
            {
                Increment32(counter);
                encryptor.TransformBlock(counter, 0, BlockSize, stream, 0);

                for (var i = 0; i < BlockSize && offset + i < length; i++)
                {
                    output[offset + i] = (Byte)(input[offset + i] ^ stream[i]);
                }
            }
        }

        //  The tag: the first counter block encrypted, XOR GHASH of the additional data, the
        //  first length bytes of the ciphertext and both of their lengths in bits.
        private static Byte[] ComputeTag(ICryptoTransform encryptor, Byte[] j0, Byte[] additionalData, Byte[] ciphertext, Int32 length)
        {
            var block = new Byte[BlockSize];
            encryptor.TransformBlock(new Byte[BlockSize], 0, BlockSize, block, 0);
            var hHi = ReadUInt64(block, 0);
            var hLo = ReadUInt64(block, 8);
            var yHi = 0UL;
            var yLo = 0UL;
            Absorb(additionalData, additionalData.Length, hHi, hLo, ref yHi, ref yLo);
            Absorb(ciphertext, length, hHi, hLo, ref yHi, ref yLo);
            yHi ^= (UInt64)additionalData.Length * 8;
            yLo ^= (UInt64)length * 8;
            Multiply(ref yHi, ref yLo, hHi, hLo);
            encryptor.TransformBlock(j0, 0, BlockSize, block, 0);
            WriteUInt64(block, 0, ReadUInt64(block, 0) ^ yHi);
            WriteUInt64(block, 8, ReadUInt64(block, 8) ^ yLo);
            return block;
        }

        //  GHASH over the data, padded with zeros to whole blocks.
        private static void Absorb(Byte[] data, Int32 length, UInt64 hHi, UInt64 hLo, ref UInt64 yHi, ref UInt64 yLo)
        {
            var block = new Byte[BlockSize];

            for (var offset = 0; offset < length; offset += BlockSize)
            {
                var count = Math.Min(BlockSize, length - offset);
                Array.Clear(block, 0, BlockSize);
                Buffer.BlockCopy(data, offset, block, 0, count);
                yHi ^= ReadUInt64(block, 0);
                yLo ^= ReadUInt64(block, 8);
                Multiply(ref yHi, ref yLo, hHi, hLo);
            }
        }

        //  Y = Y * H in GF(2^128), bit by bit with masks rather than with branches or tables, so
        //  the time taken does not depend on H (which is derived from the key).
        private static void Multiply(ref UInt64 yHi, ref UInt64 yLo, UInt64 hHi, UInt64 hLo)
        {
            var zHi = 0UL;
            var zLo = 0UL;
            var vHi = hHi;
            var vLo = hLo;

            for (var i = 0; i < 128; i++)
            {
                var bit = i < 64 ? (yHi >> (63 - i)) & 1 : (yLo >> (127 - i)) & 1;
                var mask = 0UL - bit;
                zHi ^= vHi & mask;
                zLo ^= vLo & mask;
                var carry = 0UL - (vLo & 1);
                vLo = (vLo >> 1) | (vHi << 63);
                vHi = (vHi >> 1) ^ (R & carry);
            }

            yHi = zHi;
            yLo = zLo;
        }

        //  Only the rightmost 32 bits are the counter (inc32).
        private static void Increment32(Byte[] counter)
        {
            for (var i = BlockSize - 1; i >= BlockSize - 4; i--)
            {
                if (++counter[i] != 0)
                {
                    break;
                }
            }
        }

        private static UInt64 ReadUInt64(Byte[] value, Int32 offset)
        {
            var result = 0UL;

            for (var i = 0; i < 8; i++)
            {
                result = (result << 8) | value[offset + i];
            }

            return result;
        }

        private static void WriteUInt64(Byte[] target, Int32 offset, UInt64 value)
        {
            for (var i = 7; i >= 0; i--, value >>= 8)
            {
                target[offset + i] = (Byte)value;
            }
        }
    }
}
#endif
