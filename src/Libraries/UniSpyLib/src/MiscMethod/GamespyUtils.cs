using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using UniSpyServer.UniSpyLib.Logging;

namespace UniSpyServer.UniSpyLib.MiscMethod
{
    public static class GameSpyUtils
    {
        /// <summary>
        /// Split command to key value array then convert it to dictionary
        /// </summary>
        /// <param name="request">a string like request</param>
        /// <returns></returns>
        public static Dictionary<string, string> ConvertToKeyValue(string request)
        {
            string[] commandParts = request.Replace(@"\final\", "").TrimStart('\\').Split('\\');
            return ConvertToKeyValue(commandParts);
        }

        /// </summary>
        /// Converts a trimmed presence message from the client string to a keyValue pair dictionary
        /// </summary>
        /// <param name="parts">The array of data from the client</param>
        /// <returns>A converted dictionary</returns>
        public static Dictionary<string, string> ConvertToKeyValue(string[] parts)
        {
            // we remove final item
            parts = parts.Where(p => p != "final").ToArray();
            Dictionary<string, string> dict = new Dictionary<string, string>();
            try
            {
                for (int i = 0; i < parts.Length; i += 2)
                {
                    if (!dict.ContainsKey(parts[i]))
                        dict.Add(parts[i].ToLower(), parts[i + 1]);//Some game send uppercase key to us, so we have to deal with it
                }
            }
            catch (IndexOutOfRangeException e)
            {
                LogWriter.LogError(e);
            }

            return dict;
        }


        public static void PrintReceivedGPDictToLogger(Dictionary<string, string> recv)
        {
            LogWriter.LogDebug(
                $"Received request {recv.Keys.First()} with content: {string.Join(";", recv.Select(x => x.Key + "=" + x.Value).ToArray())}");
        }

        /// <summary>
        /// Check the correctness of the email account format.
        /// </summary>
        /// <param name="email">email account</param>
        /// <returns></returns>
        public static bool IsEmailFormatCorrect(string email)
        {

            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    var domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException e)
            {
                LogWriter.LogError(e);
                return false;
            }
            catch (ArgumentException e)
            {
                LogWriter.LogError(e);
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                    @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
            //return email.Any(ch => (!char.IsLetterOrDigit(ch) && ch != '@' && ch != '.'));
        }
        /// <summary>
        /// Check is the format of nick or uniquenick are correct
        /// </summary>
        /// <param name="nick"></param>
        /// <returns></returns>
        public static bool IsNickOrUniquenickFormatCorrect(string nick)
        {
            string pattern = @"^\w+$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(nick);
        }

        /// <summary>
        /// Check if a date is correct
        /// </summary>
        /// <param name="day"></param>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns>True if the date is valid, otherwise false</returns>
        public static bool IsValidDate(int day, int month, int year)
        {
            // Check for a blank.
            /////////////////////
            if ((day == 0) && (month == 0) && (year == 0))
                return false;

            // Validate the day of the month.
            /////////////////////////////////
            switch (month)
            {
                // No month.
                ////////////
                case 0:
                    // Can't specify a day without a month.
                    ///////////////////////////////////////
                    if (day != 0)
                        return false;
                    break;

                // 31-day month.
                ////////////////
                case 1:
                case 3:
                case 5:
                case 7:
                case 8:
                case 10:
                case 12:
                    if (day > 31)
                        return false;
                    break;

                // 30-day month.
                ////////////////
                case 4:
                case 6:
                case 9:
                case 11:
                    if (day > 30)
                        return false;
                    break;

                // 28/29-day month.
                ///////////////////
                case 2:
                    // Leap year?
                    /////////////
                    if ((((year % 4) == 0) && ((year % 100) != 0)) || ((year % 400) == 0))
                    {
                        if (day > 29)
                            return false;
                    }
                    else
                    {
                        if (day > 28)
                            return false;
                    }
                    break;

                // Invalid month.
                /////////////////
                default:
                    return false;
            }

            // Check that the date is in the valid range.
            /////////////////////////////////////////////
            if (year < 1900)
                return false;
            if (year > 2079)
                return false;
            if (year == 2079)
            {
                if (month > 6)
                    return false;
                if ((month == 6) && (day > 6))
                    return false;
            }

            return true;
        }
        public static bool IsNumber(string strNumber)
        {
            Regex objNotNumberPattern = new Regex("[^0-9.-]");
            Regex objTwoDotPattern = new Regex("[0-9]*[.][0-9]*[.][0-9]*");
            Regex objTwoMinusPattern = new Regex("[0-9]*[-][0-9]*[-][0-9]*");
            string strValidRealPattern = "^([-]|[.]|[-.]|[0-9])[0-9]*[.]*[0-9]+$";
            string strValidIntegerPattern = "^([-]|[0-9])[0-9]*$";
            Regex objNumberPattern = new Regex("(" + strValidRealPattern + ")|(" + strValidIntegerPattern + ")");

            return !objNotNumberPattern.IsMatch(strNumber) &&
                   !objTwoDotPattern.IsMatch(strNumber) &&
                   !objTwoMinusPattern.IsMatch(strNumber) &&
                   objNumberPattern.IsMatch(strNumber);
        }

        /// <summary>
        /// Compute the interval of time
        /// </summary>
        /// <param name="time">old time</param>
        /// <returns>milisecond</returns>
        public static byte ComputeTimeInterval(DateTime old)
        {
            return (byte)DateTime.Now.Subtract(old).TotalMilliseconds;
        }


        /// <summary>
        /// An extension method to determine if an IP address is internal, as specified in RFC1918
        /// </summary>
        /// <param name="toTest">The IP address that will be tested</param>
        /// <returns>Returns true if the IP is internal, false if it is external</returns>
        public static bool IsInternal(this IPAddress toTest)
        {
            if (IPAddress.IsLoopback(toTest)) return true;
            else if (toTest.ToString() == "::1") return false;

            byte[] bytes = toTest.GetAddressBytes();
            switch (bytes[0])
            {
                case 10:
                    return true;
                case 172:
                    return bytes[1] < 32 && bytes[1] >= 16;
                case 192:
                    return bytes[1] == 168;
                default:
                    return false;
            }
        }
    }
}
