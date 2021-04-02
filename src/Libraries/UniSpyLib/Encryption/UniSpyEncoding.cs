using System;
using System.Text;

namespace UniSpyLib.Encryption
{
    public static class UniSpyEncoding
    {
        public static Func<byte[], string> GetString => UniSpyEncoding.GetString;
        public static Func<string, byte[]> GetBytes => UniSpyEncoding.GetBytes;

    }
}