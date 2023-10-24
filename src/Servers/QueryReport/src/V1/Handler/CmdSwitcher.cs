using System.Collections.Generic;
using UniSpy.Server.Core.Abstraction.BaseClass;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Core.Logging;
using UniSpy.Server.Core.Misc;
using UniSpy.Server.QueryReport.Application;

namespace UniSpy.Server.QueryReport.V1.Handler
{
    public sealed class CmdSwitcher : CmdSwitcherBase
    {
        private new string _rawRequest => (string)base._rawRequest;
        private new Client _client => (Client)base._client;
        public CmdSwitcher(Client client, string rawRequest) : base(client, rawRequest)
        {
        }

        protected override IHandler CreateCmdHandlers(object name, object rawRequest)
        {
            //todo add v1 support
            _client.LogError("todo add v1 support");
            switch ((string)name)
            {
                case "heartbeat":
                case "echo":
                case "validate":
                default:
                    return null;
            }
        }

        protected override void ProcessRawRequest()
        {
            // qr v1 protocol
            var name = GameSpyUtils.GetRequestName(_rawRequest);
            _requests.Add(new KeyValuePair<object, object>(name, _rawRequest));
        }
    }
}