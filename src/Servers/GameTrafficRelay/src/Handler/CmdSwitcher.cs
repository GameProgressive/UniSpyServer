using System.Collections.Generic;
using UniSpyServer.Servers.GameTrafficRelay.Entity.Structure.Request;
using UniSpyServer.Servers.GameTrafficRelay.Handler.CmdHandler;
using UniSpyServer.Servers.NatNegotiation.Entity.Enumerate;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.GameTrafficRelay.Handler
{
    public sealed class CmdSwitcher : CmdSwitcherBase
    {
        private new byte[] _rawRequest => (byte[])base._rawRequest;
        public CmdSwitcher(IClient client, object rawRequest) : base(client, rawRequest)
        {
        }
        protected override void ProcessRawRequest()
        {
            var name = (RequestType)_rawRequest[7];
            _requests.Add(new KeyValuePair<object, object>(name, _rawRequest));
        }

        protected override IHandler CreateCmdHandlers(object name, object rawRequest)
        {
            switch ((RequestType)name)
            {
                case RequestType.Ping:
                    return new PingHandler(_client, new PingRequest((byte[])rawRequest));
                default:
                    return null;
            }
        }
    }
}
