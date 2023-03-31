using System;
using System.Text;

namespace UniSpy.Server.Core.Misc
{
    public class GameSpyRandom
    {
        public enum StringType
        {
            Alpha,
            AlphaNumeric,
            Hex
        }
        /// <summary>
        /// Array of characters used in generating a signiture
        /// </summary>
        private static char[] AlphaChars = {
                'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M',
                'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'
        };

        /// <summary>
        /// An array of Alpha Numeric characters used in generating a random string
        /// </summary>
        private static char[] AlphaNumChars = {
                '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
                'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm',
                'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
                'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M',
                'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'
        };

        /// <summary>
        /// Array of Hex cahracters
        /// </summary>
        private static char[] HexChars = {
                '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
                'a', 'b', 'c', 'd', 'e', 'f'
        };

        public static string GenerateRandomString(int count, StringType type)
        {
            Random random = new Random((int)DateTime.Now.Ticks);

            StringBuilder builder = new StringBuilder(count);

            for (int i = 0; i < count; i++)
            {
                switch (type)
                {
                    case StringType.AlphaNumeric:
                        builder.Append(AlphaNumChars[random.Next(AlphaNumChars.Length)]);
                        break;
                    default:
                        builder.Append(AlphaChars[random.Next(AlphaChars.Length)]);
                        break;
                    case StringType.Hex:
                        builder.Append(HexChars[random.Next(HexChars.Length)]);
                        break;
                }
            }

            return builder.ToString();
        }
    }
}
