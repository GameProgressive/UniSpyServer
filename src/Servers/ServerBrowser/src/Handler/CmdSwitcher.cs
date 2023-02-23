using System.Collections.Generic;
using System.Linq;
using UniSpy.Server.ServerBrowser.Enumerate;
using UniSpy.Server.ServerBrowser.Contract.Request;
using UniSpy.Server.ServerBrowser.Handler.CmdHandler;
using UniSpy.Server.Core.Abstraction.BaseClass;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Core.Encryption;
using UniSpy.Server.Core.Logging;

namespace UniSpy.Server.ServerBrowser.Handler
{
    public sealed class CmdSwitcher : CmdSwitcherBase
    {
        private new byte[] _rawRequest => (byte[])base._rawRequest;

        public CmdSwitcher(IClient client, object rawRequest) : base(client, rawRequest)
        {
        }
        protected override void ProcessRawRequest()
        {
            var name = (RequestType)_rawRequest[2];
            _requests.Add(new KeyValuePair<object, object>(name, _rawRequest));
        }

        protected override IHandler CreateCmdHandlers(object name, object rawRequest)
        {
            // first we need to determine whether the first two char is \\, if yes, then it is protocol v1
            if (((byte[])rawRequest).Take(2).SequenceEqual(UniSpyEncoding.GetBytes("\\")))
            {
                var req = UniSpyEncoding.GetString((byte[])rawRequest);
                //todo add v1 support
                _client.LogError("todo add v1 support");
                return null;
            }
            // else it is protocol v2
            else
            {
                var req = (byte[])rawRequest;
                switch ((RequestType)name)
                {
                    case RequestType.ServerListRequest:
                        return new ServerListHandler(_client, new ServerListRequest(req));
                    case RequestType.ServerInfoRequest:
                        return new ServerInfoHandler(_client, new ServerInfoRequest(req));
                    case RequestType.SendMessageRequest:
                        return new SendMsgHandler(_client, new SendMsgRequest(req));
                    default:
                        return null;
                }
            }
        }
    }
}
