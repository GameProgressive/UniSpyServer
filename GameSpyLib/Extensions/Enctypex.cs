using System;
using System.Text;

namespace GameSpyLib.Extensions
{
    /// <remarks>
    /// I was never smart enough to recode this from the Open C source that Alugi made, instead
    /// i decompiled the Reality.Net.Dll from the PR guys and copied thier sources. Since this was
    /// created by using the Alugi open source code, i didnt see an harm in this.
    /// If anyone an issue with this, they can email me directly @ wilson.steven10@gmail.com.
    /// </remarks>
    public static class Enctypex
    {
        public class GSEncodingData
        {
            public byte[] EncodingKey = new byte[261];

            public long Offset;

            public long Start;
        }

        public static byte[] Encode(byte[] key, byte[] validate, byte[] data, long size)
        {
            byte[] numArray = new byte[size + 23];
            byte[] numArray1 = new byte[23];
            if (key == null || validate == null || data == null || size < 0)
            {
                return null;
            }
            int length = key.Length;
            int num = validate.Length;
            int num1 = (new System.Random()).Next();
            for (int i = 0; i < numArray1.Length; i++)
            {
                num1 = num1 * 214013 + 2531011;
                numArray1[i] = (byte)((num1 ^ key[i % length] ^ validate[i % num]) % 256);
            }
            numArray1[0] = 235;
            numArray1[1] = 0;
            numArray1[2] = 0;
            numArray1[8] = 228;
            for (long j = size - 1; j >= 0; j = j - 1)
            {
                numArray[numArray1.Length + j] = data[j];
            }
            Array.Copy(numArray1, numArray, numArray1.Length);
            size = size + numArray1.Length;
            byte[] numArray2 = Two(key, validate, numArray, size, null);
            byte[] numArray3 = new byte[numArray2.Length + numArray1.Length];
            Array.Copy(numArray1, 0, numArray3, 0, numArray1.Length);
            Array.Copy(numArray2, 0, numArray3, numArray1.Length, numArray2.Length);
            return numArray3;
        }

        private static byte[] Two(byte[] u0002, byte[] u0003, byte[] u0005, long u0008, GSEncodingData u0006)
        {
            byte[] numArray;
            byte[] numArray1 = new byte[261];
            numArray = (u0006 != null ? u0006.EncodingKey : numArray1);
            if (u0006 == null || u0006.Start == 0)
            {
                u0005 = Three(ref numArray, ref u0002, u0003, ref u0005, ref u0008, ref u0006);
                if (u0005 == null)
                {
                    return null;
                }
            }
            if (u0006 == null)
            {
                Four(ref numArray, ref u0005, u0008);
                return u0005;
            }
            if (u0006.Start == 0)
            {
                return null;
            }
            byte[] numArray2 = new byte[u0008 - u0006.Offset];
            Array.ConstrainedCopy(u0005, (int)u0006.Offset, numArray2, 0, (int)(u0008 - u0006.Offset));
            long num = Four(ref numArray, ref numArray2, u0008 - u0006.Offset);
            Array.ConstrainedCopy(numArray2, 0, u0005, (int)u0006.Offset, (int)(u0008 - u0006.Offset));
            GSEncodingData offset = u0006;
            offset.Offset = offset.Offset + num;
            byte[] numArray3 = new byte[u0008 - u0006.Start];
            Array.ConstrainedCopy(u0005, (int)u0006.Start, numArray3, 0, (int)(u0008 - u0006.Start));
            return numArray3;
        }

        private static byte[] Three(ref byte[] u0002, ref byte[] u0003, byte[] u0005, ref byte[] u0008, ref long u0006, ref GSEncodingData u000e)
        {
            long num = (long)((u0008[0] ^ 236) + 2);
            byte[] numArray = new byte[8];
            if (u0006 < (long)1)
            {
                return null;
            }
            if (u0006 < num)
            {
                return null;
            }
            long num1 = (long)(u0008[num - 1] ^ 234);
            if (u0006 < num + num1)
            {
                return null;
            }
            Array.Copy(u0005, numArray, 8);
            byte[] numArray1 = new byte[u0006 - num];
            Array.ConstrainedCopy(u0008, (int)num, numArray1, 0, (int)(u0006 - num));
            Six(ref u0002, ref u0003, ref numArray, numArray1, num1);
            Array.ConstrainedCopy(numArray1, 0, u0008, (int)num, (int)(u0006 - num));
            num = num + num1;
            if (u000e != null)
            {
                u000e.Offset = num;
                u000e.Start = num;
            }
            else
            {
                byte[] numArray2 = new byte[u0006 - num];
                Array.ConstrainedCopy(u0008, (int)num, numArray2, 0, (int)(u0006 - num));
                u0008 = numArray2;
                u0006 = u0006 - num;
            }
            return u0008;
        }

        private static long Four(ref byte[] u0002, ref byte[] u0003, long u0005)
        {
            for (long i = 0; i < u0005; i = i + 1)
            {
                u0003[i] = Five(ref u0002, u0003[i]);
            }
            return u0005;
        }

        private static byte Five(ref byte[] u0002, byte u0003)
        {
            int num = u0002[256];
            int num1 = u0002[257];
            int num2 = u0002[num];
            u0002[256] = (byte)((num + 1) % 256);
            u0002[257] = (byte)((num1 + num2) % 256);
            num = u0002[260];
            num1 = u0002[257];
            num1 = u0002[num1];
            num2 = u0002[num];
            u0002[num] = (byte)num1;
            num = u0002[259];
            num1 = u0002[257];
            num = u0002[num];
            u0002[num1] = (byte)num;
            num = u0002[256];
            num1 = u0002[259];
            num = u0002[num];
            u0002[num1] = (byte)num;
            num = u0002[256];
            u0002[num] = (byte)num2;
            num1 = u0002[258];
            num = u0002[num2];
            num2 = u0002[259];
            num1 = (num1 + num) % 256;
            u0002[258] = (byte)num1;
            num = num1;
            num2 = u0002[num2];
            num1 = u0002[257];
            num1 = u0002[num1];
            num = u0002[num];
            num2 = (num2 + num1) % 256;
            num1 = u0002[260];
            num1 = u0002[num1];
            num2 = (num2 + num1) % 256;
            num1 = u0002[num2];
            num2 = u0002[256];
            num2 = u0002[num2];
            num = (num + num2) % 256;
            num2 = u0002[num1];
            num1 = u0002[num];
            num2 = (num2 ^ num1 ^ u0003) % 256;
            u0002[260] = (byte)num2;
            u0002[259] = u0003;
            return (byte)num2;
        }

        // 78
        private static void Six(ref byte[] u0002, ref byte[] u0003, ref byte[] u0005, byte[] u0008, long u0006)
        {
            long length = (long)u0003.Length;
            for (long i = 0; i <= u0006 - 1; i = i + 1)
            {
                u0005[u0003[i % length] * i & 7] = (byte)(u0005[u0003[i % length] * i & 7] ^ u0005[i & 7] ^ u0008[i]);
            }
            long num = 8;
            Seven(ref u0002, ref u0005, ref num);
        }

        // 89
        private static void Seven(ref byte[] u0002, ref byte[] u0003, ref long u0005)
        {
            long i;
            long num = 0;
            long num1 = 0;
            if (u0005 < 1)
            {
                return;
            }
            for (i = 0; i <= 255; i = i + 1)
            {
                u0002[i] = (byte)i;
            }
            for (i = 255; i >= 0; i = i - 1)
            {
                byte num2 = (byte)Eight(u0002, i, u0003, u0005, ref num, ref num1);
                byte num3 = u0002[i];
                u0002[i] = u0002[num2];
                u0002[num2] = num3;
            }
            u0002[256] = u0002[1];
            u0002[257] = u0002[3];
            u0002[258] = u0002[5];
            u0002[259] = u0002[7];
            u0002[260] = u0002[num & 255];
        }

        // 116
        private static long Eight(byte[] u0002, long u0003, byte[] u0005, long u0008, ref long u0006, ref long u000e)
        {
            long num;
            long num1 = (long)0;
            long num2 = (long)1;
            if (u0003 == (long)0)
            {
                return 0;
            }
            if (u0003 > 1)
            {
                do
                {
                    num2 = (num2 << 1) + 1;
                }
                while (num2 < u0003);
            }
            do
            {
                u0006 = (long)(u0002[u0006 & 255] + u0005[u000e]);
                u000e = u000e + (long)1;
                if (u000e >= u0008)
                {
                    u000e = 0;
                    u0006 = u0006 + u0008;
                }
                num1 = num1 + 1;
                num = (num1 <= 11 ? u0006 & num2 : u0006 & num2 % u0003);
            }
            while (num > u0003);
            return num;
        }

        /// <summary>
        /// Encrypts / Descrypts the CDKey Query String
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string XOR(string s)
        {
            const string gamespy = "gamespy";
            int length = s.Length;
            char[] data = s.ToCharArray();
            int index = 0;

            for (int i = 0; length > 0; length--)
            {
                if (i >= gamespy.Length)
                    i = 0;

                data[index++] ^= gamespy[i++];
            }

            return new string(data);
        }

        /// <summary>
        /// simple xor encoding for Gstats,GPSP,GPCM
        /// </summary>
        /// <param name="plaintext"></param>
        /// <param name ="enc0">default encryption string used in GPSP,GPCM</param>
        /// <param name ="enc1">used in GStats</param>
        /// <param name ="enc2">used in GStats</param>
        /// <param name="enctype"></param>
        /// <returns></returns>
        public static string XorEncoding(string plaintext, int enctype)
        {
            string enc0 = "gamespy";
            string enc1 = "GameSpy3D";// '\0','a','m','e','S','p','y','3','D','\0'
            string enc2 = "Industries";// { '\0', 'n', 'd', 'u', 's', 't', 'r', 'i', 'e', 's', '\0' }
            string enc3 = "ProjectAphex";// { '\0','r','o','j','e','c','t','A','p','h','e','x','\0'}
            string statsfile = "gstats.dat";

            int length = plaintext.Length;
            char[] data = plaintext.ToCharArray();
            int index = 0;
            string temp;
            if (enctype == 0)
            {
                temp = enc0;
            }
            else if (enctype == 1)
            {
                temp = enc1;
            }
            else if (enctype == 2)
            {
                temp = enc2;
            }
            else
            {
                temp = enc3;
            }

            for (int i = 0; length > 0; length--)
            {
                if (i >= temp.Length)
                    i = 0;

                data[index++] ^= temp[i++];
            }

            return new string(data);
        }
        /// <summary>
        /// simple xor encoding for Gstats
        /// </summary>
        /// <param name="plaintext"></param>
        /// <param name="enctype"></param>
        /// <returns></returns>
        public static string XorEncoding(byte[] plainByte, int enctype)
        {
            string plaintext = Encoding.UTF8.GetString(plainByte);
            string enc0 = "gamespy";
            string enc1 = "GameSpy3D";// '\0','a','m','e','S','p','y','3','D','\0'
            string enc2 = "Industries";// { '\0', 'n', 'd', 'u', 's', 't', 'r', 'i', 'e', 's', '\0' }
            string enc3 = "ProjectAphex";// { '\0','r','o','j','e','c','t','A','p','h','e','x','\0'}
            string statsfile = "gstats.dat";

            int length = plaintext.Length;
            char[] data = plaintext.ToCharArray();
            int index = 0;
            string temp;
            if (enctype == 0)
            {
                temp = enc0;
            }
            else if (enctype == 1)
            {
                temp = enc1;
            }
            else if (enctype == 2)
            {
                temp = enc2;
            }
            else
            {
                temp = enc3;
            }

            for (int i = 0; length > 0; length--)
            {
                if (i >= temp.Length)
                    i = 0;

                data[index++] ^= temp[i++];
            }

            return new string(data);
        }

    }
}
