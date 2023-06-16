using System;
using System.Linq;
using UniSpy.Server.ServerBrowser.V1.Abstraction.BaseClass;

namespace UniSpy.Server.ServerBrowser.V1.Aggregate
{
    public class Enctype1 : EnctypeBase, IEnctype1Test
    {
        /// <summary>
        /// The server key 
        /// 256 bytes
        /// </summary>
        public static readonly byte[] Enctype1Table = new byte[] { 0x01, 0xba, 0xfa, 0xb2, 0x51, 0x00, 0x54, 0x80, 0x75, 0x16, 0x8e, 0x8e, 0x02, 0x08, 0x36, 0xa5, 0x2d, 0x05, 0x0d, 0x16, 0x52, 0x07, 0xb4, 0x22, 0x8c, 0xe9, 0x09, 0xd6, 0xb9, 0x26, 0x00, 0x04, 0x06, 0x05, 0x00, 0x13, 0x18, 0xc4, 0x1e, 0x5b, 0x1d, 0x76, 0x74, 0xfc, 0x50, 0x51, 0x06, 0x16, 0x00, 0x51, 0x28, 0x00, 0x04, 0x0a, 0x29, 0x78, 0x51, 0x00, 0x01, 0x11, 0x52, 0x16, 0x06, 0x4a, 0x20, 0x84, 0x01, 0xa2, 0x1e, 0x16, 0x47, 0x16, 0x32, 0x51, 0x9a, 0xc4, 0x03, 0x2a, 0x73, 0xe1, 0x2d, 0x4f, 0x18, 0x4b, 0x93, 0x4c, 0x0f, 0x39, 0x0a, 0x00, 0x04, 0xc0, 0x12, 0x0c, 0x9a, 0x5e, 0x02, 0xb3, 0x18, 0xb8, 0x07, 0x0c, 0xcd, 0x21, 0x05, 0xc0, 0xa9, 0x41, 0x43, 0x04, 0x3c, 0x52, 0x75, 0xec, 0x98, 0x80, 0x1d, 0x08, 0x02, 0x1d, 0x58, 0x84, 0x01, 0x4e, 0x3b, 0x6a, 0x53, 0x7a, 0x55, 0x56, 0x57, 0x1e, 0x7f, 0xec, 0xb8, 0xad, 0x00, 0x70, 0x1f, 0x82, 0xd8, 0xfc, 0x97, 0x8b, 0xf0, 0x83, 0xfe, 0x0e, 0x76, 0x03, 0xbe, 0x39, 0x29, 0x77, 0x30, 0xe0, 0x2b, 0xff, 0xb7, 0x9e, 0x01, 0x04, 0xf8, 0x01, 0x0e, 0xe8, 0x53, 0xff, 0x94, 0x0c, 0xb2, 0x45, 0x9e, 0x0a, 0xc7, 0x06, 0x18, 0x01, 0x64, 0xb0, 0x03, 0x98, 0x01, 0xeb, 0x02, 0xb0, 0x01, 0xb4, 0x12, 0x49, 0x07, 0x1f, 0x5f, 0x5e, 0x5d, 0xa0, 0x4f, 0x5b, 0xa0, 0x5a, 0x59, 0x58, 0xcf, 0x52, 0x54, 0xd0, 0xb8, 0x34, 0x02, 0xfc, 0x0e, 0x42, 0x29, 0xb8, 0xda, 0x00, 0xba, 0xb1, 0xf0, 0x12, 0xfd, 0x23, 0xae, 0xb6, 0x45, 0xa9, 0xbb, 0x06, 0xb8, 0x88, 0x14, 0x24, 0xa9, 0x00, 0x14, 0xcb, 0x24, 0x12, 0xae, 0xcc, 0x57, 0x56, 0xee, 0xfd, 0x08, 0x30, 0xd9, 0xfd, 0x8b, 0x3e, 0x0a, 0x84, 0x46, 0xfa, 0x77, 0xb8 };
        /// <summary>
        /// Client key
        /// 16 bytes
        /// </summary>
        public static readonly byte[] ClientKey = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
        public byte[] Key { get; private set; }

        byte[] IEnctype1Test.EncKey => _enckey;

        private byte[] _enckey = new byte[261];
        public Enctype1(byte[] key)
        {
            Key = key;
        }


        public override byte[] Encrypt(byte[] data)
        {
            // create a copy
            var dataCopy = data.ToArray();

            throw new System.NotImplementedException();
        }
        private byte[] Decoder(byte[] data, int dataLen)
        {
            var tbuff = new byte[326];
            var tbuff2 = new byte[258];

            var len = BitConverter.ToInt32(data.Take(4).ToArray());
            if (len <= 0)
            {
                throw new ServerBrowser.Exception("input data length must bigger than 0");
            }
            if (len > dataLen)
            {
                throw new ServerBrowser.Exception("input data length can not bigger than dataLen");
            }
            data[4] = (byte)((data[4] ^ 62) - 20);
            data[5] = (byte)((data[5] ^ 205) - 5);
            var tempData1 = data.Skip(19).ToArray();
            Func8(tempData1, 16, Enctype1Table);
            data = data.Take(19).Concat(tempData1).ToArray();
            len -= data[4] + data[5] + 40;
            var dataP = 0 + data[5] + 40;
            var tempLen = (len >> 2) - 5;
            if (tempLen >= 0)
            {
                Func4(Key);
                var tempData2 = data.Skip(dataP).ToArray();
                Func6(tempData2, tempLen);
            }


            tempLen = (len >> 1) - 17;
            if (tempLen >= 0)
            {
                var tempData3 = data.Skip(36).ToArray();
                uint[] tbuffInt = Array.ConvertAll(tbuff, Convert.ToUInt32);
                EncShare4(tempData3, 4, tbuffInt);
                data = data.Skip(36).Concat(tempData3).ToArray();
                var tempData4 = data.Skip(dataP).ToArray();
                Encshare1(tbuffInt, 0, data, dataP, tempLen);
                data = data.Skip(dataP).Concat(tempData4).ToArray();
                tbuff = Array.ConvertAll(tbuffInt, Convert.ToByte);
            }

            var tempData5 = data.Skip(19).ToArray();
            Func3(tempData5, 16, tbuff2);
            data = data.Skip(19).Concat(tempData5).ToArray();
            var tempData6 = data.Skip(dataP).ToArray();
            Func2(tempData6, len, tbuff2);
            data = data.Skip(dataP).Concat(tempData6).ToArray();
            return data;
        }
        private void Func1(byte[] data)
        {
            if (data is not null)
            {
                throw new ServerBrowser.Exception("the input data should not be null.");
            }
            if (data.Length <= 0)
            {
                throw new ServerBrowser.Exception("the input data length is not valid.");
            }
            Func4(data);

        }
        void Func2(byte[] data, int size, byte[] crypt)
        {
            byte n1, n2, t;
            n1 = crypt[256];
            n2 = crypt[257];
            int dataIndex = 0;
            while (size-- > 0)
            {
                t = crypt[++n1];
                n2 += t;
                crypt[n1] = crypt[n2];
                t += crypt[n1];
                data[dataIndex] ^= crypt[t];
                dataIndex++;
            }
            crypt[256] = n1;
            crypt[257] = n2;
        }
        void Func3(byte[] data, int len, byte[] buff)
        {
            int i;
            byte pos = 0, tmp, rev = 0xff;
            for (i = 0; i <= byte.MaxValue; i++)
            {
                buff[i] = rev--;
            }
            buff[256] = 0;
            buff[257] = 0;

            for (i = 0; i <= byte.MaxValue; i++)
            {
                tmp = buff[i];
                pos += (byte)(data[i % len] + tmp);
                buff[i] = buff[pos];
                buff[pos] = tmp;
            }
        }
        void Func4(byte[] id)
        {
            // Declare variables i, n1, n2, t1, t2
            int i,
                n1 = 0,
                n2 = 0;
            byte t1,
                t2;

            // If idlen is less than 1, return directly
            if (id.Length < 1)
                return;

            // Initialize encryption array _enc1key
            for (i = 0; i < 256; i++)
                _enckey[i] = (byte)i;

            // Shuffle the encryption array _enc1key
            for (i = 255; i >= 0; i--)
            {
                t1 = (byte)Func5(i, id, ref n1, ref n2);
                t2 = _enckey[i];
                _enckey[i] = _enckey[t1];
                _enckey[t1] = t2;
            }

            // Set specific values to some elements of the encryption array _enc1key
            _enckey[256] = _enckey[1];
            _enckey[257] = _enckey[3];
            _enckey[258] = _enckey[5];
            _enckey[259] = _enckey[7];
            _enckey[260] = _enckey[n1 & 0xff];
        }
        int Func5(int cnt, byte[] id, ref int n1, ref int n2)
        {
            // Declare variables i, tmp, mask, and initialize mask as 1
            int i,
                tmp,
                mask = 1;

            // If cnt is 0, return 0
            if (cnt == 0)
                return 0;

            // If cnt is greater than 1, increase the value of mask until it's greater than or equal to cnt
            if (cnt > 1)
            {
                do
                {
                    mask = (mask << 1) + 1;
                } while (mask < cnt);
            }

            // Initialize i as 0
            i = 0;

            // Iterate until finding a tmp value less than or equal to cnt
            do
            {
                // Update the values of n1 and n2, and calculate the tmp value
                n1 = _enckey[n1 & 0xff] + id[n2];
                n2++;
                if (n2 >= id.Length)
                {
                    n2 = 0;
                    n1 += id.Length;
                }
                tmp = n1 & mask;

                // If the iteration exceeds 11 times, take tmp modulo cnt
                if (++i > 11)
                    tmp %= cnt;
            } while (tmp > cnt);

            // Return the tmp value
            return tmp;
        }

        void Func6(byte[] data, int len)
        {
            int i = 0;
            var dataInts = Array.ConvertAll(data, Convert.ToInt32);
            while (len-- > 0)
            {
                dataInts[i] = Func7(dataInts[i]);
                i++;
            }
        }

        int Func7(int len)
        {
            // Declare variables a, b, c as unsigned char
            byte a, b, c;

            // Get certain elements from the encryption array _enc1key and calculate new values based on them
            a = _enckey[256];
            b = _enckey[257];
            c = _enckey[a];
            _enckey[256] = (byte)(a + 1);
            _enckey[257] = (byte)(b + c);

            a = _enckey[260];
            b = _enckey[257];
            b = _enckey[b];
            c = _enckey[a];
            _enckey[a] = b;

            a = _enckey[259];
            b = _enckey[257];
            a = _enckey[a];
            _enckey[b] = a;

            a = _enckey[256];
            b = _enckey[259];
            a = _enckey[a];
            _enckey[b] = a;

            a = _enckey[256];
            _enckey[a] = c;
            b = _enckey[258];
            a = _enckey[c];
            c = _enckey[259];
            b = (byte)(b + a);
            _enckey[258] = b;

            a = b;
            c = _enckey[c];
            b = _enckey[257];
            b = _enckey[b];
            a = _enckey[a];
            c += b;
            b = _enckey[260];
            b = _enckey[b];
            c += b;
            b = _enckey[c];
            c = _enckey[256];
            c = _enckey[c];
            a += c;
            c = _enckey[b];
            b = _enckey[a];

            // Store the len value in variable a and perform XOR on c and a
            a = (byte)len;
            c ^= b;
            _enckey[260] = a;
            c ^= a;
            _enckey[259] = c;

            // Return c value
            return c;
        }
        void Func8(byte[] data, int len, byte[] enctype1_data)
        {
            // Iterate over each byte in data
            var dataIndex = 0;
            while (len-- > 0)
            {
                // Encrypt the byte in data using the enctype1_data array
                data[dataIndex] = enctype1_data[dataIndex];
                dataIndex++;
            }
        }

        void IEnctype1Test.Func1(byte[] data) => Func1(data);

        void IEnctype1Test.Func2(byte[] data, int size, byte[] crypt) => Func2(data, size, crypt);

        void IEnctype1Test.Func3(byte[] data, int len, byte[] buff) => Func3(data, len, buff);

        void IEnctype1Test.Func4(byte[] id) => Func4(id);

        int IEnctype1Test.Func5(int cnt, byte[] id, ref int n1, ref int n2) => Func5(cnt, id, ref n1, ref n2);

        void IEnctype1Test.Func6(byte[] data, int len) => Func6(data, len);

        int IEnctype1Test.Func7(int len) => Func7(len);

        void IEnctype1Test.Func8(byte[] data, int len, byte[] enctype1_data) => Func8(data, len, enctype1_data);
    }
}