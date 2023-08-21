using System;
using System.Collections.Generic;
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
        public static readonly byte[] MasterKey = new byte[256] { 0x01, 0xba, 0xfa, 0xb2, 0x51, 0x00, 0x54, 0x80, 0x75, 0x16, 0x8e, 0x8e, 0x02, 0x08, 0x36, 0xa5, 0x2d, 0x05, 0x0d, 0x16, 0x52, 0x07, 0xb4, 0x22, 0x8c, 0xe9, 0x09, 0xd6, 0xb9, 0x26, 0x00, 0x04, 0x06, 0x05, 0x00, 0x13, 0x18, 0xc4, 0x1e, 0x5b, 0x1d, 0x76, 0x74, 0xfc, 0x50, 0x51, 0x06, 0x16, 0x00, 0x51, 0x28, 0x00, 0x04, 0x0a, 0x29, 0x78, 0x51, 0x00, 0x01, 0x11, 0x52, 0x16, 0x06, 0x4a, 0x20, 0x84, 0x01, 0xa2, 0x1e, 0x16, 0x47, 0x16, 0x32, 0x51, 0x9a, 0xc4, 0x03, 0x2a, 0x73, 0xe1, 0x2d, 0x4f, 0x18, 0x4b, 0x93, 0x4c, 0x0f, 0x39, 0x0a, 0x00, 0x04, 0xc0, 0x12, 0x0c, 0x9a, 0x5e, 0x02, 0xb3, 0x18, 0xb8, 0x07, 0x0c, 0xcd, 0x21, 0x05, 0xc0, 0xa9, 0x41, 0x43, 0x04, 0x3c, 0x52, 0x75, 0xec, 0x98, 0x80, 0x1d, 0x08, 0x02, 0x1d, 0x58, 0x84, 0x01, 0x4e, 0x3b, 0x6a, 0x53, 0x7a, 0x55, 0x56, 0x57, 0x1e, 0x7f, 0xec, 0xb8, 0xad, 0x00, 0x70, 0x1f, 0x82, 0xd8, 0xfc, 0x97, 0x8b, 0xf0, 0x83, 0xfe, 0x0e, 0x76, 0x03, 0xbe, 0x39, 0x29, 0x77, 0x30, 0xe0, 0x2b, 0xff, 0xb7, 0x9e, 0x01, 0x04, 0xf8, 0x01, 0x0e, 0xe8, 0x53, 0xff, 0x94, 0x0c, 0xb2, 0x45, 0x9e, 0x0a, 0xc7, 0x06, 0x18, 0x01, 0x64, 0xb0, 0x03, 0x98, 0x01, 0xeb, 0x02, 0xb0, 0x01, 0xb4, 0x12, 0x49, 0x07, 0x1f, 0x5f, 0x5e, 0x5d, 0xa0, 0x4f, 0x5b, 0xa0, 0x5a, 0x59, 0x58, 0xcf, 0x52, 0x54, 0xd0, 0xb8, 0x34, 0x02, 0xfc, 0x0e, 0x42, 0x29, 0xb8, 0xda, 0x00, 0xba, 0xb1, 0xf0, 0x12, 0xfd, 0x23, 0xae, 0xb6, 0x45, 0xa9, 0xbb, 0x06, 0xb8, 0x88, 0x14, 0x24, 0xa9, 0x00, 0x14, 0xcb, 0x24, 0x12, 0xae, 0xcc, 0x57, 0x56, 0xee, 0xfd, 0x08, 0x30, 0xd9, 0xfd, 0x8b, 0x3e, 0x0a, 0x84, 0x46, 0xfa, 0x77, 0xb8 };
        /// <summary>
        /// Client key
        /// 16 bytes
        /// </summary>
        public static readonly byte[] ClientKey = new byte[16] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
        public byte[] ValidateKey { get; private set; }
        private byte[] _sboxInitSeed = BitConverter.GetBytes(0);
        private uint[] _sbox = new uint[326];
        private byte[] _enc0Key = new byte[258];
        private byte[] _enc1Key = new byte[261];
        /// <summary>
        /// The enc0seed is using to generate enc0key
        /// </summary>
        private byte[] _enc0Seed = Enumerable.Repeat<byte>(0x01, 16).ToArray();
        /// <summary>
        /// _enc0seedIndex is the index of enc0seed, so we find the index in master key,
        /// we only use the 0 index value in master key so we hard code the index
        /// </summary>
        private byte[] _enc0seedIndex = Enumerable.Repeat<byte>(0x00, 16).ToArray();
        public Enctype1(byte[] validateKey)
        {
            ValidateKey = validateKey;
        }

        public override byte[] Encrypt(byte[] data)
        {
            // create a copy
            var plaintext = data.ToArray();

            if (plaintext.Length <= 1)
            {
                throw new ServerBrowser.Exception("The data can not be empty");
            }
            int tempLen = (plaintext.Length >> 1) - 17;
            if (tempLen >= 0)
            {
                ConstructSbox(_sboxInitSeed, _sboxInitSeed.Length, _sbox);
                ChangeSboxEncryptPlaintext(_sbox, plaintext, 0, tempLen);
            }
            CreateEnc0Key(_enc0Seed, _enc0Key);
            EncryptByEnc0Key(plaintext, plaintext.Length, _enc0Key);
            tempLen = (plaintext.Length >> 2) - 5;
            if (tempLen >= 0)
            {
                CreateEnc1Key(ValidateKey, _enc1Key);
                EncryptByEnc1Key(plaintext, tempLen, _enc1Key);
            }
            var ciphertext = BuildOutput(ValidateKey, _enc0seedIndex, plaintext, _sboxInitSeed);
            return ciphertext;
        }

        private byte[] BuildOutput(byte[] validateKey, byte[] scrambleData, byte[] encryptedData, byte[] sboxInitSeed)
        {
            var output = new List<byte>();
            // 4 bytes message length
            // newOutput.AddRange(new byte[4] { 0x00, 0x00, 0x00, 0x00 });
            output.Add(42);
            output.Add(218);
            // 6 to 19 is unknown data, we do not use it
            output.AddRange(Enumerable.Repeat<byte>(0, 13).ToArray());
            output.AddRange(scrambleData);
            // unused data
            output.AddRange(new byte[4] { 0x00, 0x00, 0x00, 0x00 });
            // sboxInitSeed
            output.AddRange(sboxInitSeed);
            // unused data
            output.AddRange(Enumerable.Repeat<byte>(0x00, 18).ToArray());

            output.AddRange(encryptedData);
            // insert encryption length to index 0
            output.InsertRange(0, BitConverter.GetBytes(output.Count).Reverse());
            return output.ToArray();
        }
        private byte[] Decoder(byte[] data, int dataLen)
        {
            // var tbuff = new byte[326];
            // var tbuff2 = new byte[258];

            // var len = BitConverter.ToInt32(data.Take(4).ToArray());
            // if (len <= 0)
            // {
            //     throw new ServerBrowser.Exception("input data length must bigger than 0");
            // }
            // if (len > dataLen)
            // {
            //     throw new ServerBrowser.Exception("input data length can not bigger than dataLen");
            // }
            // data[4] = (byte)((data[4] ^ 62) - 20);
            // data[5] = (byte)((data[5] ^ 205) - 5);
            // var tempData1 = data.Skip(19).ToArray();
            // Func8(tempData1, 16, Enctype1Table);
            // data = data.Take(19).Concat(tempData1).ToArray();
            // len -= data[4] + data[5] + 40;
            // var dataP = 0 + data[5] + 40;
            // var tempLen = (len >> 2) - 5;
            // if (tempLen >= 0)
            // {
            //     ChangeEncKeyByValidateKey(Key,enckey);
            //     var tempData2 = data.Skip(dataP).ToArray();
            //     Func6(tempData2, tempLen);
            // }


            // tempLen = (len >> 1) - 17;
            // if (tempLen >= 0)
            // {
            //     var tempData3 = data.Skip(36).ToArray();
            //     uint[] tbuffInt = Array.ConvertAll(tbuff, Convert.ToUInt32);
            //     ConstructSbox(tempData3, 4, tbuffInt);
            //     data = data.Skip(36).Concat(tempData3).ToArray();
            //     var tempData4 = data.Skip(dataP).ToArray();
            //     ChangeSboxEncryptPlaintext(tbuffInt, 0, data, dataP, tempLen);
            //     data = data.Skip(dataP).Concat(tempData4).ToArray();
            //     tbuff = Array.ConvertAll(tbuffInt, Convert.ToByte);
            // }

            // var tempData5 = data.Skip(19).ToArray();
            // CreateEncryptionKey(tempData5, tbuff2);
            // data = data.Skip(19).Concat(tempData5).ToArray();
            // var tempData6 = data.Skip(dataP).ToArray();
            // EncryptByEncKey(tempData6, len, tbuff2);
            // data = data.Skip(dataP).Concat(tempData6).ToArray();
            // return data;
            throw new System.NotImplementedException();
        }
        void EncryptByEnc0Key(byte[] data, int size, byte[] enc0Key)
        {
            byte n1, n2, t;
            n1 = enc0Key[256];
            n2 = enc0Key[257];
            int dataIndex = 0;
            while (size-- > 0)
            {
                t = enc0Key[++n1];
                n2 += t;
                enc0Key[n1] = enc0Key[n2];
                enc0Key[n2] = t;

                t += enc0Key[n1];
                data[dataIndex] ^= enc0Key[t];
                dataIndex++;
            }
            enc0Key[256] = n1;
            enc0Key[257] = n2;
        }
        void CreateEnc0Key(byte[] scrambleData, byte[] enc0Key)
        {
            int i;
            byte pos = 0, tmp, rev = 0xff;
            for (i = 0; i <= byte.MaxValue; i++)
            {
                enc0Key[i] = rev--;
            }
            enc0Key[256] = 0;
            enc0Key[257] = 0;

            for (i = 0; i <= byte.MaxValue; i++)
            {
                tmp = enc0Key[i];
                pos += (byte)(scrambleData[i % scrambleData.Length] + tmp);
                enc0Key[i] = enc0Key[pos];
                enc0Key[pos] = tmp;
            }
        }
        void CreateEnc1Key(byte[] validateKey, byte[] enc1Key)
        {
            // Declare variables i, n1, n2, t1, t2
            int i,
                n1 = 0,
                n2 = 0;
            byte t1,
                t2;

            // If idlen is less than 1, return directly
            if (validateKey.Length < 1)
                return;

            // Initialize encryption array _enc1key
            for (i = 0; i < 256; i++)
                enc1Key[i] = (byte)i;

            // Shuffle the encryption array _enc1key
            for (i = 255; i >= 0; i--)
            {
                t1 = (byte)Func5(i, validateKey, ref n1, ref n2, enc1Key);
                t2 = enc1Key[i];
                enc1Key[i] = enc1Key[t1];
                enc1Key[t1] = t2;
            }

            // Set specific values to some elements of the encryption array _enc1key
            enc1Key[256] = enc1Key[1];
            enc1Key[257] = enc1Key[3];
            enc1Key[258] = enc1Key[5];
            enc1Key[259] = enc1Key[7];
            enc1Key[260] = enc1Key[n1 & 0xff];
        }
        int Func5(int cnt, byte[] id, ref int n1, ref int n2, byte[] encKey)
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
                n1 = encKey[n1 & 0xff] + id[n2];
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
        /// <summary>
        /// init the enckey, data is not touched
        /// </summary>
        void EncryptByEnc1Key(byte[] data, int len, byte[] enc1Key)
        {
            int i = 0;
            while (len-- > 0)
            {
                data[i] = (byte)SubstituteEnc1key(data[i], enc1Key);
                i++;
            }
        }
        int SubstituteEnc1key(int len, byte[] enc1Key)
        {
            // Declare variables a, b, c as unsigned char
            byte a, b, c;

            // Get certain elements from the encryption array _enc1key and calculate new values based on them
            a = enc1Key[256];
            b = enc1Key[257];
            c = enc1Key[a];
            enc1Key[256] = (byte)(a + 1);
            enc1Key[257] = (byte)(b + c);

            a = enc1Key[260];
            b = enc1Key[257];
            b = enc1Key[b];
            c = enc1Key[a];
            enc1Key[a] = b;

            a = enc1Key[259];
            b = enc1Key[257];
            a = enc1Key[a];
            enc1Key[b] = a;

            a = enc1Key[256];
            b = enc1Key[259];
            a = enc1Key[a];
            enc1Key[b] = a;

            a = enc1Key[256];
            enc1Key[a] = c;
            b = enc1Key[258];
            a = enc1Key[c];
            c = enc1Key[259];
            b = (byte)(b + a);
            enc1Key[258] = b;

            a = b;
            c = enc1Key[c];
            b = enc1Key[257];
            b = enc1Key[b];
            a = enc1Key[a];
            c += b;
            b = enc1Key[260];
            b = enc1Key[b];
            c += b;
            b = enc1Key[c];
            c = enc1Key[256];
            c = enc1Key[c];
            a += c;
            c = enc1Key[b];
            b = enc1Key[a];

            // Store the len value in variable a and perform XOR on c and a
            a = (byte)len;
            c ^= b;
            enc1Key[260] = a;
            c ^= a;
            enc1Key[259] = c;

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
                data[dataIndex] = enctype1_data[data[dataIndex]];
                dataIndex++;
            }
        }

        void EncryptScrambleData(byte[] scrambleData, byte[] masterKey)
        {
            for (int i = 0; i < scrambleData.Length; i++)
            {
                byte offset = FindIndexInMasterKey(scrambleData[i], masterKey);
                scrambleData[i] = offset;
            }
        }
        byte FindIndexInMasterKey(byte data, byte[] masterKey)
        {
            for (byte i = 0; i < masterKey.Length; i++)
            {
                if (data == masterKey[i])
                {
                    return i;
                }
            }
            throw new System.Exception("No index found for scramble data");
        }

        void IEnctype1Test.EncryptByEnc0Key(byte[] data, int size, byte[] crypt) => EncryptByEnc0Key(data, size, crypt);
        void IEnctype1Test.CreateEnc0Key(byte[] data, int len, byte[] buff) => CreateEnc0Key(data, buff);
        void IEnctype1Test.CreateEnc1Key(byte[] validateKey, byte[] enc1Key) => CreateEnc1Key(validateKey, enc1Key);
        int IEnctype1Test.Func5(int cnt, byte[] id, ref int n1, ref int n2, byte[] encKey) => Func5(cnt, id, ref n1, ref n2, encKey);
        void IEnctype1Test.EncryptByEnc1Key(byte[] data, int len, byte[] enc1Key) => EncryptByEnc1Key(data, len, enc1Key);
        int IEnctype1Test.SubstituteEnc1key(int len, byte[] enc1Key) => SubstituteEnc1key(len, enc1Key);
        void IEnctype1Test.Func8(byte[] data, int len, byte[] enctype1_data) => Func8(data, len, enctype1_data);
        byte[] IEnctype1Test.Enc0Key => _enc0Key;
        byte[] IEnctype1Test.Enc1Key => _enc1Key;
        byte[] IEnctype1Test.ValidateKey => ValidateKey;
    }
}