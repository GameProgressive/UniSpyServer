using System;
using System.Text;

namespace UniSpyServer.UniSpyLib.Encryption
{
    public static class UniSpyEncoding
    {
        public static Func<byte[], string> GetString => Encoding.ASCII.GetString;
        public static Func<string, byte[]> GetBytes => Encoding.ASCII.GetBytes;

    }
}