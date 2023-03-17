using System.Collections.Generic;
using UniSpy.Server.ServerBrowser.Enumerate;
using UniSpy.Server.ServerBrowser.Contract.Request;
using UniSpy.Server.ServerBrowser.Handler.CmdHandler;
using UniSpy.Server.Core.Abstraction.BaseClass;
using UniSpy.Server.Core.Abstraction.Interface;

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
