using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Net.Sockets;
using GameSpyLib.Network;
using GameSpyLib.Logging;

namespace GameSpyLib.Common
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

        /// <summary>
        /// Converts a trimmed presence message from the client string to a keyValue pair dictionary
        /// </summary>
        /// <param name="parts">The array of data from the client</param>
        /// <returns>A converted dictionary</returns>
        public static Dictionary<string, string> ConvertGPResponseToKeyValue(string[] parts)
        {
            Dictionary<string, string> Data = new Dictionary<string, string>();
            try
            {
                for (int i = 0; i < parts.Length; i += 2)
                {
                    if (!Data.ContainsKey(parts[i]))
                        Data.Add(parts[i], parts[i + 1]);
                }
            }
            catch (IndexOutOfRangeException) { }

            return Data;
        }

        public static void PrintReceivedGPDictToLogger(string req, Dictionary<string, string> dict)
        {
            LogWriter.Log.Write("Received request {0} with content: {1}", LogLevel.Debug, req, string.Join(";", dict.Select(x => x.Key + "=" + x.Value).ToArray()));
        }

        /// <summary>
        /// Send a presence error
        /// </summary>
        /// <param name="stream">The stream that will receive the error</param>
        /// <param name="code">The error code</param>
        /// <param name="error">A string containing the error</param>
        public static void SendGPError(TcpPacket handler, int code, string error)
        {
            handler.SendAsync(Encoding.UTF8.GetBytes(string.Format(@"\error\\err\{0}\fatal\\errmsg\{1}\id\1\final\", code, error)));
        }

        /// <summary>
        /// Send a presence error
        /// </summary>
        /// <param name="socket">The socket that will receive the error</param>
        /// <param name="code">The error code</param>
        /// <param name="error">A string containing the error</param>
        public static void SendGPError(Socket socket, int code, string error)
        {
            if (LogWriter.Log.DebugSockets)
                LogWriter.Log.Write("Sending TCP data: {0}", LogLevel.Debug, string.Format(@"\error\\err\{0}\fatal\\errmsg\{1}\id\1\final\", code, error));

            socket.Send(Encoding.UTF8.GetBytes(string.Format(@"\error\\err\{0}\fatal\\errmsg\{1}\id\1\final\", code, error)));
        }
        /// <summary>
        /// Check the correctness of the email account format.
        /// </summary>
        /// <param name="email">email account</param>
        /// <returns></returns>
        public static bool IsEmailFormatCorrect(string email)
        {
            if (email.Contains('@'))
            {
                //a correct email format contains the postfix.
                if (email.Contains(".cn") || email.Contains(".com") || email.Contains(".net"))
                {
                    //a correct email format can not contain #$%^&*()!
                    if (!(email.Contains('#') && email.Contains('&') && email.Contains('$') && email.Contains('*') && email.Contains('(') && email.Contains(')') && email.Contains('!') && email.Contains('^')))
                    {
                        return true;
                    }
                }
            }
            return false;
        }


        ///// <summary>
        ///// Check a message sended by TCP whether have multiple commands
        ///// </summary>
        ///// <param name="message"></param>
        ///// <returns></returns>
        //public static int CheckGPMultiCommands(string message)
        //{
        //    string keyWord = @"\final\";
        //    //set index to the start position, and count of commands to zero;
        //    int index = 0;
        //    int commandCount = 0;

        //    while ((index = message.IndexOf(keyWord, index)) != -1)
        //    {
        //        //set index to position where the end index of the keyword:@"\final\" 
        //        index = index + message.IndexOf(keyWord, index) + keyWord.Length;
        //        //if a keyword:@"\final\" is found then count one;
        //        commandCount++;
        //    }

        //    return commandCount;
        //}
        ///// <summary>
        ///// Split multiple command send by single TCP stream and return the subcommand
        ///// </summary>
        ///// <param name="message"></param>
        ///// <returns></returns>
        //public static string[] SplitMultipleCommand(string message)
        //{
        //    string keyWord = @"\final\";
        //    int preindex = 0;
        //    int index = 0;
        //    int commandCount = 0;
        //    string[] submessage = new string[commandCount];
        //    while ((index = message.IndexOf(keyWord, index)) != -1)
        //    {
        //        //set index to position where the end index of the keyword:@"\final\" 
        //        index = index + message.IndexOf(keyWord, index) + keyWord.Length;
        //        //set the substring when first keyword is found
        //        submessage[commandCount-1] = message.Substring(preindex, index);
        //        //set the preindex to the current index,
        //        //when next time the index become the end of next keyword,
        //        //the preindex will be the next start position.
        //        preindex = index;
        //        commandCount++;
        //    }

        //    return submessage;
        //}
    }
}
