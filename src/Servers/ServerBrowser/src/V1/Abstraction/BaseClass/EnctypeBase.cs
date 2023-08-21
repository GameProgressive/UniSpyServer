using System;
using System.Linq;
using UniSpy.Server.Core.Abstraction.Interface;

namespace UniSpy.Server.ServerBrowser.V1.Abstraction.BaseClass
{
    /// <summary>
    /// The encryption type 1 and 2 base class which contains the functions using in enctype1 and enctype2.
    /// What confuses me the most is why gamespy doesn't use standard encryption schemes like DES, AES etc.
    /// </summary>
    public abstract class EnctypeBase : ICryptography, IEnctypeShareTest
    {
        public byte[] Decrypt(byte[] data) => throw new UniSpy.Exception("Enctype only encrypt message on server side.");
        public abstract byte[] Encrypt(byte[] data);
        /// <summary>
        /// The encryption function 2
        /// </summary>
        /// <param name="sbox">the input tbuff</param>
        /// <param name="sboxIndex">tbuff start index</param>
        /// <param name="data">datap is a subset of tbuff</param>
        /// <param name="startIndex">datap start index</param>
        /// <param name="len">unknown</param>
        protected void ChangeSboxEncryptPlaintext(uint[] sbox, byte[] data, int startIndex, int len)
        {
            int pIndex, sIndex;
            // convert uint array to byte array
            pIndex = sIndex = 309 * sizeof(uint);
            SboxElementExchange(sbox, 309, 16);
            var sboxBytes = ConvertUintToBytes(sbox);

            while (len-- > 0)
            {
                if ((pIndex - sIndex) == 63)
                {
                    pIndex = sIndex;
                    //convert int array to byte array
                    SboxElementExchange(sbox, 0, 16);
                    sboxBytes = ConvertUintToBytes(sbox);
                }
                data[startIndex] ^= sboxBytes[pIndex];
                startIndex++;
                pIndex++;
            }
            sbox = ConvertBytesToUint(sboxBytes);
        }
        /// <summary>
        /// Encryption related function2
        /// </summary>
        /// <param name="sbox">the input data</param>
        /// <param name="startIndex">the another index of input data</param>
        /// <param name="len">unknown</param>
        protected void SboxElementExchange(uint[] sbox, uint startIndex, int len)
        {
            uint t1, t2, t3, t4, t5;
            uint limit;
            uint p;

            t2 = sbox[304];
            t1 = sbox[305];
            t3 = sbox[306];
            t5 = sbox[307];
            limit = (uint)(startIndex + len);
            while (startIndex < limit)
            {
                p = t2 + 272;
                while (t5 <= ushort.MaxValue)
                {
                    t1 += t5;
                    p++;
                    t3 += t1;
                    t1 += t3;
                    sbox[p - 17] = t1;
                    sbox[p - 1] = t3;
                    t4 = (t3 << 24) | (t3 >> 8);
                    sbox[p + 15] = t5;
                    t5 <<= 1;
                    t2++;
                    t1 ^= sbox[t1 & 0xff];
                    t4 ^= sbox[t4 & 0xff];
                    t3 = (t4 << 24) | (t4 >> 8);
                    t4 = (t1 >> 24) | (t1 << 8);
                    t4 ^= sbox[t4 & 0xff];
                    t3 ^= sbox[t3 & 0xff];
                    t1 = (t4 >> 24) | (t4 << 8);
                }
                t3 ^= t1;
                sbox[startIndex++] = t3;
                t2--;
                t1 = sbox[t2 + 256];
                t5 = sbox[t2 + 272];
                t1 = ~t1;
                t3 = (t1 << 24) | (t1 >> 8);
                t3 ^= sbox[t3 & 0xff];
                t5 ^= sbox[t5 & 0xff];
                t1 = (t3 << 24) | (t3 >> 8);
                t4 = (t5 >> 24) | (t5 << 8);
                t1 ^= sbox[t1 & 0xff];
                t4 ^= sbox[t4 & 0xff];
                t3 = (t4 >> 24) | (t4 << 8);
                t5 = (sbox[t2 + 288] << 1) + 1;
            }

            sbox[304] = t2;
            sbox[305] = t1;
            sbox[306] = t3;
            sbox[307] = t5;
        }
        protected void EncShare3(uint[] data, int n1 = 0, int n2 = 0)
        {
            uint t1, t2, t3, t4;
            int i;

            t2 = (uint)n1;
            t1 = 0;
            t4 = 1;
            data[304] = 0;

            for (i = 32768; i != 0; i >>= 1)
            {
                t2 += t4;
                t1 += t2;
                t2 += t1;

                if ((n2 & i) != 0)
                {
                    t2 = ~t2;
                    t4 = (t4 << 1) + 1;
                    t3 = (t2 << 24) | (t2 >> 8);
                    t3 ^= data[t3 & 0xFF];
                    t1 ^= data[t1 & 0xFF];
                    t2 = (t3 << 24) | (t3 >> 8);
                    t3 = (t1 >> 24) | (t1 << 8);
                    t2 ^= data[t2 & 0xFF];
                    t3 ^= data[t3 & 0xFF];
                    t1 = (t3 >> 24) | (t3 << 8);
                }
                else
                {
                    data[data[304] + 256] = t2;
                    data[data[304] + 272] = t1;
                    data[data[304] + 288] = t4;
                    data[304]++;
                    t3 = (t1 << 24) | (t1 >> 8);
                    t2 ^= data[t2 & 0xFF];
                    t3 ^= data[t3 & 0xFF];
                    t1 = (t3 << 24) | (t3 >> 8);
                    t3 = (t2 >> 24) | (t2 << 8);
                    t3 ^= data[t3 & 0xFF];
                    t1 ^= data[t1 & 0xFF];
                    t2 = (t3 >> 24) | (t3 << 8);
                    t4 <<= 1;
                }
            }

            data[305] = t2;
            data[306] = t1;
            data[307] = t4;
            data[308] = (uint)n1;
        }
        /// <summary>
        /// Seems this function is used to initialize encryption parameter
        /// </summary>
        /// <param name="input">the sbox seed</param>
        /// <param name="size">seed size</param>
        /// <param name="output">sbox</param>
        protected void ConstructSbox(byte[] input, int size, uint[] output)
        {
            uint tmp;
            int i;
            byte pos, x, y;

            for (i = 0; i < 256; i++)
                output[i] = 0;

            for (y = 0; y < 4; y++)
            {
                for (i = 0; i < 256; i++)
                {
                    output[i] = (output[i] << 8) + (uint)i;
                }

                for (pos = y, x = 0; x < 2; x++)
                {
                    for (i = 0; i < 256; i++)
                    {
                        tmp = output[i];
                        pos += (byte)(tmp + input[i % size]);
                        output[i] = output[pos];
                        output[pos] = tmp;
                    }
                }
            }

            for (i = 0; i < 256; i++)
                output[i] ^= (uint)i;

            EncShare3(output);
        }
        protected static byte[] ConvertUintToBytes(uint[] input)
        {
            var ontputBytes = new byte[input.Length * sizeof(uint)];
            for (var i = 0; i < input.Length; i++)
            {
                Array.Copy(BitConverter.GetBytes(input[i]), 0, ontputBytes, i * sizeof(uint), sizeof(uint));
            }
            return ontputBytes.ToArray();
        }
        protected static uint[] ConvertBytesToUint(byte[] input)
        {
            if (input.Length % sizeof(uint) != 0)
            {
                throw new System.Exception("the input length is not correct");
            }
            var onputInts = new uint[input.Length / sizeof(uint)];
            for (var i = 0; i < (input.Length / sizeof(uint)); i++)
            {
                var tempBytes = input.Skip(i * sizeof(uint)).Take(sizeof(uint)).ToArray();
                var tempUint = BitConverter.ToUInt32(tempBytes);
                onputInts[i] = tempUint;
            }
            return onputInts;
        }

        void IEnctypeShareTest.Encshare1(uint[] tbuff, byte[] datap, int datapIndex, int len) => ChangeSboxEncryptPlaintext(tbuff, datap, datapIndex, len);

        void IEnctypeShareTest.Encshare2(uint[] tbuff, uint tbuffp, int len) => SboxElementExchange(tbuff, tbuffp, len);

        void IEnctypeShareTest.EncShare3(uint[] data, int n1, int n2) => EncShare3(data, n1, n2);

        void IEnctypeShareTest.EncShare4(byte[] src, int size, uint[] dest) => ConstructSbox(src, size, dest);
        uint[] IEnctypeShareTest.ConvertBytesToUint(byte[] input) => ConvertBytesToUint(input);
        byte[] IEnctypeShareTest.ConvertUintToBytes(uint[] input) => ConvertUintToBytes(input);

    }
}