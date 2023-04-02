using System.Collections.Generic;
using UniSpy.Server.Core.Abstraction.BaseClass;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Core.Encryption;
using UniSpy.Server.Core.Logging;
using UniSpy.Server.Core.Misc;

namespace UniSpy.Server.ServerBrowser.V1.Handler
{
    public sealed class CmdSwitcher : CmdSwitcherBase
    {
        private new string _rawRequest => UniSpyEncoding.GetString((byte[])base._rawRequest);

        public CmdSwitcher(IClient client, object rawRequest) : base(client, rawRequest)
        {
        }

        protected override IHandler CreateCmdHandlers(object name, object rawRequest)
        {
            //todo add v1 support
            _client.LogError("todo add v1 support");
            switch ((string)name)
            {
                case "list":
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