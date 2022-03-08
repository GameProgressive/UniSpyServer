using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using UniSpyServer.UniSpyLib.Encryption;

namespace UniSpyServer.UniSpyLib.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Returns all the index's of the given string input
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static List<int> IndexesOf(this string str, string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("the string to find may not be empty", "value");
            List<int> indexes = new List<int>();
            for (int index = 0; ; index += value.Length)
            {
                index = str.IndexOf(value, index);
                if (index == -1)
                    return indexes;
                indexes.Add(index);
            }
        }

        /// <summary>
        /// Removes any invalid file path characters from this string
        /// </summary>
        public static string MakeFileNameSafe(this string fileName)
        {
            return Path.GetInvalidFileNameChars().Aggregate(fileName, (current, c) => current.Replace(c.ToString(), string.Empty));
        }

        /// <summary>
        /// Converts the input string into its MD5 Hex variant
        /// </summary>
        /// <param name="upperCase">Uppercase the characters?</param>
        /// <param name="Encoding">The encoding of the string. Default is UTF8</param>
        /// <returns></returns>
        public static string GetMD5Hash(this string input, bool upperCase = false, Encoding Encoding = null)
        {
            using (MD5 Md5 = MD5.Create())
            {
                return Md5.ComputeHash(Encoding.ASCII.GetBytes(input)).ToHex(upperCase);
            }
        }
        /// <summary>
        /// Converts the byte array to its Hex string equivlent
        /// </summary>
        /// <param name="upperCase">Do we uppercase the hex string?</param>
        /// <returns></returns>
        public static string ToHex(this byte[] bytes, bool upperCase = true)
        {
            StringBuilder result = new StringBuilder(bytes.Length * 2);
            for (int i = 0; i < bytes.Length; i++)
                result.Append(bytes[i].ToString(upperCase ? "X2" : "x2"));

            return result.ToString();
        }
        #region Undisplayable char convert
        /// <summary>
        /// Replace unreadable charactors to Hex for logging
        /// </summary>
        /// <param name="message"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string ReplaceUnreadableCharToHex(byte[] message, long length)
        {
            byte[] tempMessage = new byte[length];
            Array.Copy(message, tempMessage, length);
            return ReplaceUnreadableCharToHex(tempMessage);
        }
        public static string ReplaceUnreadableCharToHex(byte[] buffer)
        {
            StringBuilder temp = new StringBuilder();
            for (int i = 0; i < buffer.Length; i++)
            {
                if (buffer[i] < 0x1F || buffer[i] > 0x7E)
                {
                    temp.Append($"[{buffer[i]:X2}]");
                }
                else
                {
                    temp.Append((char)buffer[i]);
                }
            }
            return temp.ToString();
        }
        public static string ReplaceUnreadableCharToHex(string buffer)
        {
            return ReplaceUnreadableCharToHex(UniSpyEncoding.GetBytes(buffer));
        }
        #endregion

        public static Dictionary<string, string> ConvertKVStringToDictionary(string kvStr)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            List<string> keyValueList =
               kvStr.Split("\\").ToList();

            for (int i = 0; i < keyValueList.Count; i = i + 2)
            {
                if (keyValueList.Count < i + 2) // Count starts from 1, i from 0
                {
                    dic.TryAdd(keyValueList[i], "");
                }
                else
                {
                    dic.TryAdd(keyValueList[i], keyValueList[i + 1]);
                }
            }

            return dic;
        }
        public static string ConvertDictionaryToKVString(Dictionary<string, string> kv)
        {
            string buffer = @"\";
            foreach (var data in kv)
            {
                buffer += data.Key + @"\" + data.Value + @"\";
            }

            buffer = buffer.Substring(0, buffer.Length - 1);

            return buffer;
        }

        public static List<string> ConvertKeyStrToList(string keyStr)
        {
            List<string> data = keyStr.Split(@"\", StringSplitOptions.RemoveEmptyEntries).ToList();

            return data;
        }

        /// <summary>
        /// Check the validation of response string
        /// </summary>
        /// <returns>return true if the string is valid</returns>
        public static bool CheckResponseValidation(string buffer)
        {
            return buffer?.Length > 3;
        }

        public static bool CheckResponseValidation(byte[] buffer)
        {
            if (buffer == null)
            {
                return false;
            }

            if (buffer.Length < 3)
            {
                return false;
            }
            return true;
        }
    }
}
