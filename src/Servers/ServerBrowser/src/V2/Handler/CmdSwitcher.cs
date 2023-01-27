using System.Collections.Generic;
using System.Linq;
using UniSpyServer.Servers.ServerBrowser.V2.Entity.Enumerate;
using UniSpyServer.Servers.ServerBrowser.V2.Entity.Exception;
using UniSpyServer.Servers.ServerBrowser.V2.Entity.Structure.Request;
using UniSpyServer.Servers.ServerBrowser.V2.Handler.CmdHandler;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Encryption;
using UniSpyServer.UniSpyLib.Logging;

namespace UniSpyServer.Servers.ServerBrowser.V2.Handler
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
                    // currently we only support natneg client message
                    // we need more test to see whether there are other games that sends other client message
                    case RequestType.NatNegMsgRequest:
                        goto case RequestType.SendMessageRequest;
                    default:
                        return null;
                }
            }
        }
    }
}
