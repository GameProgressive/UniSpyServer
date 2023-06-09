using System.Net.NetworkInformation;
using System;
using System.Linq;
using UniSpy.Server.Core.Abstraction.Interface;

namespace UniSpy.Server.ServerBrowser.V1.Abstraction.BaseClass
{
    /// <summary>
    /// The encryption type 1 and 2 base class which contains the functions using in enctype1 and enctype2.
    /// What confuses me the most is why gamespy doesn't use standard encryption schemes like DES, AES etc.
    /// </summary>
    public abstract class EnctypeBase : ICryptography
    {
        public byte[] Decrypt(byte[] data) => throw new UniSpy.Exception("Enctype only encrypt message on server side.");
        public abstract byte[] Encrypt(byte[] data);
        protected void Encshare1(uint[] tbuff, byte[] datap, int len)
        {
            int pIndex, sIndex;
            // convert uint array to byte array
            var tbuffBytes = Array.ConvertAll(tbuff, Convert.ToByte);
            var datapIndex = 0;
            pIndex = sIndex = 309 * 4;
            Encshare2(tbuff, tbuff.Skip(pIndex / 4).ToArray(), 16);
            while (len-- > 0)
            {
                if ((pIndex - sIndex) == 63)
                {
                    pIndex = sIndex;
                    //convert int array to byte array
                    var newtbuffBytes = Array.ConvertAll(tbuff, Convert.ToUInt32);
                    Encshare2(tbuff, newtbuffBytes, 16);
                }
                datap[datapIndex] ^= tbuffBytes[pIndex];
                datapIndex++;
                pIndex++;
            }
        }
        protected void Encshare2(uint[] tbuff, uint[] tbuffp, int len)
        {

            uint t1, t2, t3, t4, t5;
            uint limit;
            uint p;

            t2 = tbuff[304];
            t1 = tbuff[305];
            t3 = tbuff[306];
            t5 = tbuff[307];
            limit = (uint)len;
            // Array.Copy(tbuffp, limit, len);
            // for (int i = 0; i < len; i++)
            // {

            // }
            uint tbuffpIndex = 0;
            while (tbuffpIndex < limit)
            {
                p = t2 + 272;
                while (t5 < (ushort.MaxValue + 1))
                {
                    t1 += t5;
                    p++;
                    t3 += t1;
                    t1 += t3;
                    tbuff[p - 17] = t1;
                    tbuff[p - 1] = t1;
                    t4 = (t3 << 24) | (t3 >> 8);
                    tbuff[p + 15] = t1;
                    t5 <<= 1;
                    t2++;
                    t1 ^= tbuff[t1 & 0xff];
                    t4 ^= tbuff[t4 & 0xff];
                    t3 = (t4 << 24) | (t4 >> 8);
                    t4 = (t1 >> 24) | (t1 << 8);
                    t4 ^= tbuff[t4 & 0xff];
                    t3 ^= tbuff[t3 & 0xff];
                    t1 = (t4 >> 24) | (t4 << 8);
                }
                t3 ^= t1;
                tbuff[tbuffpIndex] = t3;
                tbuffpIndex++;
                t2--;
                t1 = tbuff[t2 + 256];
                t5 = tbuff[t2 + 272];
                t1 = ~t1;
                t3 = (t1 << 24) | (t1 >> 8);
                t3 ^= tbuff[t3 & 0xff];
                t5 ^= tbuff[t5 & 0xff];
                t1 = (t3 << 24) | (t3 >> 8);
                t4 = (t5 >> 24) | (t5 << 8);
                t1 ^= tbuff[t1 & 0xff];
                t4 ^= tbuff[t4 & 0xff];
                t3 = (t4 >> 24) | (t4 << 8);
                t5 = (tbuff[t2 + 288] << 1) + 1;
            }

            tbuff[304] = t2;
            tbuff[305] = t1;
            tbuff[306] = t3;
            tbuff[307] = t5;
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
        protected void EncShare4(byte[] src, int size, uint[] dest)
        {
            uint tmp;
            int i;
            byte pos, x, y;

            for (i = 0; i < 256; i++) // Initialize all elements in dest array to 0
                dest[i] = 0;

            for (y = 0; y < 4; y++) // Loop 4 times, process one byte at a time
            {
                for (i = 0; i < 256; i++) // Perform operation on each element in dest array
                {
                    dest[i] = (dest[i] << 8) + (uint)i; // Left shift dest[i] by 8 bits and add i in the low 8 bits
                }

                for (pos = y, x = 0; x < 2; x++) // Loop 2 times, process 128 bytes each time
                {
                    for (i = 0; i < 256; i++) // Perform operation on each element in dest array
                    {
                        tmp = dest[i];                            // Save the value of dest[i] in tmp
                        pos += (byte)(tmp + src[i % size]);       // Calculate the value of pos
                        dest[i] = dest[pos];                      // Assign the value of dest[pos] to dest[i]
                        dest[pos] = tmp;                          // Assign the value of tmp to dest[pos]
                    }
                }
            }

            for (i = 0; i < 256; i++) // Perform operation on each element in dest array
                dest[i] ^= (uint)i;    // Perform XOR between dest[i] and i to obtain the final key

            EncShare3(dest); // Call the encshare3 function to further encrypt the key
        }
    }
}