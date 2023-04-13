using System.Collections.Generic;
using UniSpy.Server.Core.Abstraction.BaseClass;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Core.Encryption;
using UniSpy.Server.Core.Misc;
using UniSpy.Server.ServerBrowser.V1.Application;
using UniSpy.Server.ServerBrowser.V1.Contract.Request;
using UniSpy.Server.ServerBrowser.V1.Handler.CmdHandler;

namespace UniSpy.Server.ServerBrowser.V1.Handler
{
    public sealed class CmdSwitcher : CmdSwitcherBase
    {
        private new Client _client => (Client)base._client;
        private new string _rawRequest => UniSpyEncoding.GetString((byte[])base._rawRequest);

        public CmdSwitcher(IClient client, object rawRequest) : base(client, rawRequest)
        {
        }

        protected override IHandler CreateCmdHandlers(object name, object rawRequest)
        {
            //todo add v1 support
            var request = (string)rawRequest;
            switch ((string)name)
            {
                case "gamename":
                    return new GameNameHandler(_client, new GameNameRequest(request));
                case "list":
                    return new ListHandler(_client, new ListRequest(request));
                default:
                    return null;
            }
        }

        protected override void ProcessRawRequest()
        {
            // sb v1 protocol
            var name = GameSpyUtils.GetRequestName(_rawRequest);
            _requests.Add(new KeyValuePair<object, object>(name, _rawRequest));
        }
    }
}