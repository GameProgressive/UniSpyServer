using UniSpy.Server.Core.Abstraction.Interface;

namespace UniSpy.Server.ServerBrowser.V1.Abstraction.BaseClass
{
    public abstract class EnctypeBase : ICryptography
    {
        public byte[] Decrypt(byte[] data) => throw new UniSpy.Exception("Enctype only encrypt message on server side.");

        public abstract byte[] Encrypt(byte[] data);


        protected void EncShare3(byte[] data, byte n1 = 0, byte n2 = 0)
        {
            byte t1, t2, t3, t4;
            int i;
            t2 = n1;
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
                    t2 = (byte)~t2;
                    t4 = (byte)((t4 << 1) + 1);
                    t3 = (byte)((t2 << 24) | (t2 >> 8));
                    t3 ^= data[t3 & 0xff];
                    t1 ^= data[t1 & 0xff];
                    t2 = (byte)((t3 << 24) | (t3 >> 8));
                    t3 = (byte)((t1 >> 24) | (t1 << 8));
                    t2 ^= data[t2 & 0xff];
                    t3 ^= data[t3 & 0xff];
                    t1 = (byte)((t3 >> 24) | (t3 << 8));
                }
                else
                {
                    data[data[304] + 256] = t2;
                    data[data[304] + 272] = t1;
                    data[data[304] + 288] = t4;
                    data[304]++;
                    t3 = (byte)((t1 << 24) | (t1 >> 8));
                    t2 ^= data[t2 & 0xff];
                    t3 ^= data[t3 & 0xff];
                    t1 = (byte)((t3 << 24) | (t3 >> 8));
                    t3 = (byte)((t2 >> 24) | (t2 << 8));
                    t3 ^= data[t3 & 0xff];
                    t1 ^= data[t1 & 0xff];
                    t2 = (byte)((t3 >> 24) | (t3 << 8));
                    t4 <<= 1;
                }
            }
            data[305] = t2;
            data[306] = t1;
            data[307] = t4;
            data[308] = n1;
        }
        protected byte[] EncShare4(byte[] data)
        {
            byte[] encodeData = new byte[326];
            byte tmp;
            byte i;
            byte pos, x, y;
            // encrypted data size
            int size = data[0];

            for (y = 0; y < 4; y++)
            {
                for (i = 0; i <= 255; i++)
                {
                    encodeData[i] = (byte)((encodeData[i] << 8) + i);
                }

                for (pos = y, x = 0; x < 2; x++)
                {
                    for (i = 0; i <= 255; i++)
                    {
                        tmp = encodeData[i];
                        pos += (byte)(tmp + data[i % size]);
                        encodeData[i] = encodeData[pos];
                        encodeData[pos] = tmp;
                    }
                }
            }

            for (i = 0; i <= 255; i++)
            {
                encodeData[i] ^= i;
            }

            EncShare3(encodeData);
            return encodeData;
        }

    }
}