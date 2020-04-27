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
                    temp.Append(string.Format("[{0:X2}]", buffer[i]));
                }
                else
                {
                    temp.Append((char)buffer[i]);
                }
            }
            return temp.ToString();
        }

        public static void ShowRetroSpyLogo(string version)
        {
            Console.WriteLine(@"  ___     _           ___             ___                      ");
            Console.WriteLine(@" | _ \___| |_ _ _ ___/ __|_ __ _  _  / __| ___ _ ___ _____ _ _ ");
            Console.WriteLine(@" |   / -_)  _| '_/ _ \__ \ '_ \ || | \__ \/ -_) '_\ V / -_) '_|");
            Console.WriteLine(@" |_|_\___|\__|_| \___/___/ .__/\_, | |___/\___|_|  \_/\___|_|  ");
            Console.WriteLine(@"                         |_|   |__/                            ");
            Console.WriteLine(@" Version: " + version);

        }

        public static string FormatServerTableContext(string part1, string part2, string part3)
        {
            return string.Format("|{0,-11}|{1,-14}|{2,-6}|", part1, part2, part3);
        }
        public static string FormatServerTableHeader(string part1, string part2, string part3)
        {
            return string.Format("+{0,-11}+{1,-14}+{2,-6}+", part1, part2, part3);
        }


        public static Dictionary<string, string> ConvertKVStrToDic(string kvStr)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            List<string> keyValueList =
               kvStr.TrimStart('\\').Split("\\", StringSplitOptions.RemoveEmptyEntries).ToList();

            for (int i = 0; i < keyValueList.Count; i = i + 2)
            {
                dic.TryAdd(keyValueList[i], keyValueList[i + 1]);
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
            List<string> data = keyStr.Split(@"\",StringSplitOptions.RemoveEmptyEntries).ToList();

           return data;
        }
    }
}
