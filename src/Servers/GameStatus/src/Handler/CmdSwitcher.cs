using System.Collections.Generic;
using System.Linq;
using UniSpyServer.Servers.GameStatus.Entity.Structure.Request;
using UniSpyServer.Servers.GameStatus.Handler.CmdHandler;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Encryption;
using UniSpyServer.UniSpyLib.MiscMethod;

namespace UniSpyServer.Servers.GameStatus.Handler
{
    public sealed class CmdSwitcher : CmdSwitcherBase
    {
        private new string _rawRequest => UniSpyEncoding.GetString((byte[])base._rawRequest);

        public CmdSwitcher(IClient client, object rawRequest) : base(client, rawRequest)
        {
        }
        protected override void ProcessRawRequest()
        {
            if (_rawRequest[0] != '\\')
            {
                throw new UniSpyException("Invalid request");
            }

            var name = GameSpyUtils.ConvertToKeyValue(_rawRequest).Keys.First();
            _requests.Add(new KeyValuePair<object, object>(name, _rawRequest));
        }

        protected override IHandler CreateCmdHandlers(object name, object rawRequest)
        {
            switch ((string)name)
            {
                case "auth":
                    return new AuthGameHandler(_client, new AuthGameRequest((string)rawRequest));
                case "authp":
                    return new AuthPlayerHandler(_client, new AuthPlayerRequest((string)rawRequest));
                case "newgame":
                    return new CreateNewGameHandler(_client, new CreateNewGameRequest((string)rawRequest));
                case "getpd":
                    return new GetPlayerDataHandler(_client, new GetPlayerDataRequest((string)rawRequest));
                case "setpd":
                    return new SetPlayerDataHandler(_client, new SetPlayerDataRequest((string)rawRequest));
                case "updgame":
                    return new UpdateGameHandler(_client, new UpdateGameRequest((string)rawRequest));
                default:
                    return null;
            }
        }
    }
}
