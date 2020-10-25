using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace GameSpyLib.Extensions
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
            if (String.IsNullOrEmpty(value))
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
                if (Encoding == null) Encoding = Encoding.UTF8;
                return Md5.ComputeHash(Encoding.GetBytes(input)).ToHex(upperCase);
            }
        }
        /// <summary>
        /// Replace unreadable charactors to ? for logging
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static string ReplaceUnreadableCharToHex(byte[] buffer, int index, int size)
        {
            return ReplaceUnreadableCharToHex(buffer.Take(size).ToArray());
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
            return ReplaceUnreadableCharToHex(Encoding.ASCII.GetBytes(buffer));
        }

        public static string FormatServerTableContext(string part1, string part2, string part3)
        {
            return $"|{part1,-11}|{part2,-14}|{part3,-6}|";
        }
        public static string FormatServerTableHeader(string part1, string part2, string part3)
        {
            return $"+{part1,-11}+{part2,-14}+{part3,-6}+";
        }


        public static Dictionary<string, string> ConvertKVStrToDic(string kvStr)
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
        public static string ConvertDicKVToStr(Dictionary<string, string> kv)
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
            if (buffer == null || buffer == "" || buffer.Length < 3)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool CheckResponseValidation(byte[] buffer)
        {
            if (buffer == null && buffer.Length < 3)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
