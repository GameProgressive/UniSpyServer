using System;
using UniSpy.Server.ServerBrowser.V1.Abstraction.BaseClass;

namespace UniSpy.Server.ServerBrowser.V1.Aggregate
{
    public class Enctype1 : EnctypeBase
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
        private byte[] _enc1key = new byte[261];
        public Enctype1(byte[] key)
        {
            Key = key;
        }


        public override byte[] Encrypt(byte[] data)
        {
            throw new System.NotImplementedException();
        }

        private void Func1(byte[] id)
        {
            if (id is not null)
            {
                if (id.Length > 0)
                {
                    Func4(id);
                }
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
            byte[] _enc1key = new byte[256];
            for (i = 0; i < 256; i++)
                _enc1key[i] = (byte)i;

            // Shuffle the encryption array _enc1key
            for (i = 255; i >= 0; i--)
            {
                t1 = (byte)Func5(i, id, ref n1, ref n2);
                t2 = _enc1key[i];
                _enc1key[i] = _enc1key[t1];
                _enc1key[t1] = t2;
            }

            // Set specific values to some elements of the encryption array _enc1key
            _enc1key[256] = _enc1key[1];
            _enc1key[257] = _enc1key[3];
            _enc1key[258] = _enc1key[5];
            _enc1key[259] = _enc1key[7];
            _enc1key[260] = _enc1key[n1 & 0xff];
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
                n1 = _enc1key[n1 & 0xff] + id[n2];
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

        private void Func6(byte[] data)
        {
            var len = data.Length;
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
            a = _enc1key[256];
            b = _enc1key[257];
            c = _enc1key[a];
            _enc1key[256] = (byte)(a + 1);
            _enc1key[257] = (byte)(b + c);

            a = _enc1key[260];
            b = _enc1key[257];
            b = _enc1key[b];
            c = _enc1key[a];
            _enc1key[a] = b;

            a = _enc1key[259];
            b = _enc1key[257];
            a = _enc1key[a];
            _enc1key[b] = a;

            a = _enc1key[256];
            b = _enc1key[259];
            a = _enc1key[a];
            _enc1key[b] = a;

            a = _enc1key[256];
            _enc1key[a] = c;
            b = _enc1key[258];
            a = _enc1key[c];
            c = _enc1key[259];
            b = (byte)(b + a);
            _enc1key[258] = b;

            a = b;
            c = _enc1key[c];
            b = _enc1key[257];
            b = _enc1key[b];
            a = _enc1key[a];
            c += b;
            b = _enc1key[260];
            b = _enc1key[b];
            c += b;
            b = _enc1key[c];
            c = _enc1key[256];
            c = _enc1key[c];
            a += c;
            c = _enc1key[b];
            b = _enc1key[a];

            // Store the len value in variable a and perform XOR on c and a
            a = (byte)len;
            c ^= b;
            _enc1key[260] = a;
            c ^= a;
            _enc1key[259] = c;

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

    }
}