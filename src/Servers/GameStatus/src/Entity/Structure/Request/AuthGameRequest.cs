using UniSpy.Server.GameStatus.Abstraction.BaseClass;
using UniSpy.Server.GameStatus.Entity.Exception;

namespace UniSpy.Server.GameStatus.Entity.Structure.Request
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
            if (!PlayerData.ContainsKey("gamename"))
            {
                throw new GSException("gamename is missing.");
            }

            if (!PlayerData.ContainsKey("response"))
            {
                throw new GSException("response is missing.");
            }

            if (PlayerData.ContainsKey("port"))
            {
                int port;
                if (!int.TryParse(PlayerData["port"], out port))
                {
                    throw new GSException("port format is incorrect.");
                }
                Port = port;
            }

            GameName = PlayerData["gamename"];
        }
    }
}
