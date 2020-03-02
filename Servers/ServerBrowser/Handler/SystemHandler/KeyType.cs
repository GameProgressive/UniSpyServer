using System;
using ServerBrowser.Entity.Enumerator;

namespace ServerBrowser.Handler.SystemHandler
{
    public class KeyType
    {
        public static SBKeyType GetKeyType(byte[] data)
        {
            if (data.Length == 1)
            {
                return SBKeyType.Byte;
            }
            else if (data.Length == 2)
            {
                return SBKeyType.Short;
            }
            else
            {
                return SBKeyType.String;
            }
        }
    }
}
