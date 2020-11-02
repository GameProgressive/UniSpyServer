using ServerBrowser.Entity.Enumerate;

namespace ServerBrowser.Handler.SystemHandler.KeyType
{
    /// <summary>
    /// all unique value string has 255 limits,
    /// we do not think there will be a lot of server for each game
    /// so we parse every value in string format for simplicity
    /// </summary>
    public class KeyTypeHandler
    {
        public SBKeyType GetKeyType(string key, string value)
        {
            if (IsByte(key, value))
            {
                return SBKeyType.Byte;
            }
            else if (IsShort(key, value))
            {
                return SBKeyType.Short;
            }
            else
            {
                return SBKeyType.String;
            }
        }

        public bool IsByte(string key, string value)
        {
            if (value.Length == 1)
            {
                if (key.Contains("numplayers") || key.Contains("maxplayers") || key.Contains("password"))
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsShort(string key, string value)
        {
            if (value.Length == 2)
            {
                return true;
            }

            return false;
        }
    }
}
