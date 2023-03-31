using System;
using System.Collections.Generic;
using System.Text;
using UniSpy.Server.Core.Abstraction.BaseClass;
using UniSpy.Server.Core.Encryption;
using UniSpy.Server.Core.Extension;

namespace UniSpy.Server.Core.Misc
{
    public class PasswordEncoder
    {
        /// <summary>
        /// process password to string which stores in our database
        /// </summary>
        /// <param name="dict"></param>
        public static string ProcessPassword(Dictionary<string, string> dict)
        {
            string md5Password;
            if (dict.ContainsKey("passwordenc"))
            {
                //we decoded gamespy encoded password then get md5 of it         
                md5Password = StringExtensions.GetMD5Hash(Decode(dict["passwordenc"]));
            }
            else if (dict.ContainsKey("passenc"))
            {
                //we decoded gamespy encoded password then get md5 of it  
                md5Password = StringExtensions.GetMD5Hash(Decode(dict["passenc"]));
            }
            else if (dict.ContainsKey("pass"))
            {
                md5Password = StringExtensions.GetMD5Hash(dict["pass"]);
            }
            else if (dict.ContainsKey("password"))
            {
                md5Password = StringExtensions.GetMD5Hash(dict["password"]);
            }
            else
            {
                throw new UniSpyException("Can not find password field in request");
            }
            return md5Password;
        }
        /// <summary>
        /// Encodes a password to Gamespy format
        /// </summary>
        /// <param name="Password"></param>
        /// <returns></returns>
        private static string Encode(string Password)
        {
            // Get password string as UTF8 String, Convert to Base64
            byte[] PasswordBytes = Encoding.UTF8.GetBytes(Password);
            string Pass = Convert.ToBase64String(GameSpyEncodeMethod(PasswordBytes));

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
        private static string Decode(string password)
        {
            // Convert Gamespy Base64 to Standard Base 64
            password = password.Replace('_', '=').Replace('[', '+').Replace(']', '/');
            // Decode passsword
            byte[] passwordBytes = Convert.FromBase64String(password);
            return UniSpyEncoding.GetString(GameSpyEncodeMethod(passwordBytes));
        }

        /// <summary>
        /// Gamespy's XOR method to encrypt and decrypt a password
        /// </summary>
        /// <param name="pass"></param>
        /// <returns></returns>
        private static byte[] GameSpyEncodeMethod(byte[] pass)
        {
            int a = 0;
            int num = 0x79707367; // gamespy
            for (int i = 0; i < pass.Length; ++i)
            {
                num = GameSpyByteShift(num);
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
        private static int GameSpyByteShift(int num)
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
