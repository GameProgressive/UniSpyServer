using System;
using System.Collections.Generic;
using System.Linq;
using UniSpy.Server.PresenceSearchPlayer.Contract.Request;
using UniSpy.Server.PresenceSearchPlayer.Handler.CmdHandler;
using UniSpy.Server.Core.Abstraction.BaseClass;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Core.Logging;
using UniSpy.Server.PresenceSearchPlayer.Application;

namespace UniSpy.Server.PresenceSearchPlayer.Handler
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
            if (_rawRequest[0] != '\\')
            {
                _client.LogInfo("Invalid request recieved!");
                return;
            }
            string[] rawRequests = _rawRequest.Split("\\final\\", StringSplitOptions.RemoveEmptyEntries);
            foreach (var rawRequest in rawRequests)
            {
                var name = rawRequest.TrimStart('\\').Split("\\").First();
                _requests.Add(new KeyValuePair<object, object>(name, rawRequest));
            }
        }

        protected override IHandler CreateCmdHandlers(object name, object rawRequest)
        {
            switch ((string)name)
            {
                case "check":
                    return new CheckHandler(_client, new CheckRequest((string)rawRequest));
                case "newuser":
                    return new NewUserHandler(_client, new NewUserRequest((string)rawRequest));
                case "nicks":
                    return new NicksHandler(_client, new NicksRequest((string)rawRequest));
                case "others":
                    return new OthersHandler(_client, new OthersRequest((string)rawRequest));
                case "otherslist":
                    return new OthersListHandler(_client, new OthersListRequest((string)rawRequest));
                case "pmatch":
                    // return new PMatchHandler(_client, new PMatchRequest((string)rawRequest));
                    throw new NotImplementedException();
                case "search":
                    return new SearchHandler(_client, new SearchRequest((string)rawRequest));
                case "searchunique":
                    return new SearchUniqueHandler(_client, new SearchUniqueRequest((string)rawRequest));
                case "uniquesearch":
                    return new UniqueSearchHandler(_client, new UniqueSearchRequest((string)rawRequest));
                case "valid":
                    return new ValidHandler(_client, new ValidRequest((string)rawRequest));
                default:
                    return null;
            }
        }
    }
}
