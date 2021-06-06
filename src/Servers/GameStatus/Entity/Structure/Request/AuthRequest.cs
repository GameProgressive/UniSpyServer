using GameStatus.Abstraction.BaseClass;
using GameStatus.Entity.Enumerate;
using GameStatus.Entity.Exception;

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


            if (!RequestKeyValues.ContainsKey("gamename"))
            {
                throw new GSException("gamename is missing.");
            }

            if (!RequestKeyValues.ContainsKey("response"))
            {
                throw new GSException("response is missing.");
            }

            if (RequestKeyValues.ContainsKey("port"))
            {
                uint port;
                if (!uint.TryParse(RequestKeyValues["port"], out port))
                {
                    throw new GSException("port format is incorrect.");
                }
                Port = port;
            }

            GameName = RequestKeyValues["gamename"];
        }
    }
}
