using UniSpy.Server.GameStatus.Abstraction.BaseClass;
using UniSpy.Server.GameStatus.Exception;

namespace UniSpy.Server.GameStatus.Contract.Request
{
    /// <summary>
    /// Request: //auth\\gamename\%s\response\%s\port\%d\id\1 */
    /// </summary>

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
            if (!KeyValues.ContainsKey("lid") && !KeyValues.ContainsKey("id"))
            {
                throw new GSException("localid is missing.");
            }
            base.Parse();
            if (!KeyValues.ContainsKey("gamename"))
            {
                throw new GSException("gamename is missing.");
            }

            if (!KeyValues.ContainsKey("response"))
            {
                throw new GSException("response is missing.");
            }

            if (KeyValues.ContainsKey("port"))
            {
                int port;
                if (!int.TryParse(KeyValues["port"], out port))
                {
                    throw new GSException("port format is incorrect.");
                }
                Port = port;
            }

            GameName = KeyValues["gamename"];
        }
    }
}
