using System;
using System.Linq;
using System.Collections.Generic;
using UniSpy.Server.GameStatus.Contract.Request;
using UniSpy.Server.GameStatus.Handler.CmdHandler;
using UniSpy.Server.Core.Abstraction.BaseClass;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Core.Encryption;
using UniSpy.Server.Core.Logging;
using UniSpy.Server.GameStatus.Application;

namespace UniSpy.Server.GameStatus.Handler
{
    public sealed class CmdSwitcher : CmdSwitcherBase
    {
        private new string _rawRequest => (string)base._rawRequest;
        private new Client _client => (Client)base._client;
        public CmdSwitcher(Client client, string rawRequest) : base(client, rawRequest)
        {
        }
        protected override void ProcessRawRequest()
        {
            var rawRequests = _rawRequest.Split(@"\final\", StringSplitOptions.RemoveEmptyEntries);

            foreach (var rawRequest in rawRequests)
            {
                if (_rawRequest[0] != '\\')
                {
                    // throw new UniSpy.Exception("Invalid request");
                    _client.LogError($"Invaid request: {rawRequest}");
                    continue;
                }

                var name = rawRequest.TrimStart('\\').Split('\\').First();
                _requests.Add(new KeyValuePair<object, object>(name, rawRequest));
            }
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
                    return new NewGameHandler(_client, new NewGameRequest((string)rawRequest));
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
