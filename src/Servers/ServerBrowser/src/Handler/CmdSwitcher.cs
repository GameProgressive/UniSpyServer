using System.Collections.Generic;
using UniSpyServer.Servers.ServerBrowser.Entity.Enumerate;
using UniSpyServer.Servers.ServerBrowser.Entity.Structure.Request;
using UniSpyServer.Servers.ServerBrowser.Handler.CmdHandler;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.ServerBrowser.Handler
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
            switch ((RequestType)name)
            {
                case RequestType.ServerListRequest:
                    return new ServerListHandler(_client, new ServerListRequest((byte[])rawRequest));
                case RequestType.NatNegRequest:
                    return new NatNegMsgHandler(_client, new NatNegMsgRequest((byte[])rawRequest));
                case RequestType.ServerInfoRequest:
                    return new ServerInfoHandler(_client, new ServerInfoRequest((byte[])rawRequest));
                case RequestType.SendMessageRequest:
                    return new SendMsgHandler(_client, new SendMsgRequest((byte[])rawRequest));
                default:
                    return null;
            }
        }
    }
}
