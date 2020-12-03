using GameStatus.Abstraction.BaseClass;
using GameStatus.Entity.Enumerate;
using System.Collections.Generic;

namespace GameStatus.Entity.Structure.Request
{
    /// <summary>
    /// Request: //auth\\gamename\%s\response\%s\port\%d\id\1 */
    /// </summary>
    public class AuthRequest : GSRequestBase
    {
        public string GameName { get; protected set; }
        public uint Port { get; protected set; }
        public AuthRequest(string rawRequest) : base(rawRequest)
        {
        }

        public override object Parse()
        {
           var flag = (GSError)base.Parse();
            if (flag != GSError.NoError)
            {
                return flag;
            }

            if (!KeyValues.ContainsKey("gamename") && !KeyValues.ContainsKey("response"))
            {
                return GSError.Parse;
            }

            if (KeyValues.ContainsKey("port"))
            {
                uint port;
                if (!uint.TryParse(KeyValues["port"], out port))
                {
                    return GSError.Parse;
                }
                Port = port;
            }

            GameName = KeyValues["gamename"];

            return GSError.NoError;
        }
    }
}
