using GameStatus.Abstraction.BaseClass;
using GameStatus.Entity.Enumerate;

namespace GameStatus.Entity.Structure.Request
{
    /// <summary>
    /// Request: //auth\\gamename\%s\response\%s\port\%d\id\1 */
    /// </summary>
    internal sealed class AuthRequest : GSRequestBase
    {
        public string GameName { get; private set; }
        public uint Port { get; private set; }
        public AuthRequest(string rawRequest) : base(rawRequest)
        {
        }

        public override void Parse()
        {
            base.Parse();
            if (ErrorCode != GSErrorCode.NoError)
            {
                return;
            }

            if (!RequestKeyValues.ContainsKey("gamename") && !RequestKeyValues.ContainsKey("response"))
            {
                ErrorCode = GSErrorCode.Parse;
                return;
            }

            if (RequestKeyValues.ContainsKey("port"))
            {
                uint port;
                if (!uint.TryParse(RequestKeyValues["port"], out port))
                {
                    ErrorCode = GSErrorCode.Parse;
                    return;
                }
                Port = port;
            }

            GameName = RequestKeyValues["gamename"];
        }
    }
}
