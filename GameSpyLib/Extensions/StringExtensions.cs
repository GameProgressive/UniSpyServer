using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

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
            return ReplaceUnreadableCharToHex(Encoding.ASCII.GetString(buffer, index, size));
        }

        public static string ReplaceUnreadableCharToHex(string buffer)
        {
           return Regex.Replace(buffer,
                          @"\p{Cc}",
                          a => string.Format("[{0:X2}]", (byte)a.Value[0])
                        );
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
    }
}
