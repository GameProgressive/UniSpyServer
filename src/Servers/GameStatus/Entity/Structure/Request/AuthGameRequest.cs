using GameStatus.Abstraction.BaseClass;
using GameStatus.Entity.Contract;
using GameStatus.Entity.Exception;

namespace GameStatus.Entity.Structure.Request
{
    /// <summary>
    /// Request: //auth\\gamename\%s\response\%s\port\%d\id\1 */
    /// </summary>
    [RequestContract("auth")]
    internal sealed class AuthGameRequest : RequestBase
    {
        public string GameName { get; private set; }
        public uint Port { get; private set; }
        public AuthGameRequest(string rawRequest) : base(rawRequest)
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
