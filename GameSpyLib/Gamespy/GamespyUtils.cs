using System;
using System.Text;

namespace GameSpyLib
{
    public static class GamespyUtils
    {
        /// <summary>
        /// Encodes a password to Gamespy format
        /// </summary>
        /// <param name="Password"></param>
        /// <returns></returns>
        public static string EncodePassword(string Password)
        {
            // Get password string as UTF8 String, Convert to Base64
            byte[] PasswordBytes = Encoding.UTF8.GetBytes(Password);
            string Pass = Convert.ToBase64String(GsPassEncode(PasswordBytes));

            // Convert Standard Base64 to Gamespy Base 64
            StringBuilder builder = new StringBuilder(Pass);
            builder.Replace('=', '_');
            builder.Replace('+', '[');
            builder.Replace('/', ']');
            return builder.ToString();
        }

        /// <summary>
        /// Decodes a Gamespy encoded password
        /// </summary>
        /// <param name="Password"></param>
        /// <returns></returns>
        public static string DecodePassword(string Password)
        {
            // Convert Gamespy Base64 to Standard Base 64
            StringBuilder builder = new StringBuilder(Password);
            builder.Replace('_', '=');
            builder.Replace('[', '+');
            builder.Replace(']', '/');

            // Decode passsword
            byte[] PasswordBytes = Convert.FromBase64String(builder.ToString());
            return Encoding.UTF8.GetString(GsPassEncode(PasswordBytes));
        }

        /// <summary>
        /// Gamespy's XOR method to encrypt and decrypt a password
        /// </summary>
        /// <param name="pass"></param>
        /// <returns></returns>
        public static byte[] GsPassEncode(byte[] pass)
        {
            int a = 0;
            int num = 0x79707367; // gspy
            for (int i = 0; i < pass.Length; ++i)
            {
                num = Gslame(num);
                a = num % 0xFF;
                pass[i] ^= (byte)a;
            }

            return pass;
        }

        /// <summary>
        /// Not exactly sure what this does, but i know its used to 
        /// reverse the encryption and decryption of a string
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        private static int Gslame(int num)
        {
            int c = (num >> 16) & 0xffff;
            int a = num & 0xffff;

            c *= 0x41a7;
            a *= 0x41a7;
            a += ((c & 0x7fff) << 16);

            if (a < 0)
            {
                a &= 0x7fffffff;
                a++;
            }

            a += (c >> 15);

            if (a < 0)
            {
                a &= 0x7fffffff;
                a++;
            }

            return a;
        }
    }
}
