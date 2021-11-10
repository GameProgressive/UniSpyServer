using UniSpyServer.Servers.GameStatus.Abstraction.BaseClass;
using UniSpyServer.Servers.GameStatus.Entity.Contract;
using UniSpyServer.Servers.GameStatus.Entity.Exception;

namespace UniSpyServer.Servers.GameStatus.Entity.Structure.Request
{
    /// <summary>
    /// Request: //auth\\gamename\%s\response\%s\port\%d\id\1 */
    /// </summary>
    [RequestContract("auth")]
    public sealed class AuthGameRequest : RequestBase
    {
        public string GameName { get; private set; }
        public int Port { get; private set; }
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
                int port;
                if (!int.TryParse(RequestKeyValues["port"], out port))
                {
                    throw new GSException("port format is incorrect.");
                }
                Port = port;
            }

            GameName = RequestKeyValues["gamename"];
        }
    }
}
