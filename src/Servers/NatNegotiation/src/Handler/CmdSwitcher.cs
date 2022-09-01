using System.Collections.Generic;
using UniSpyServer.Servers.NatNegotiation.Entity.Enumerate;
using UniSpyServer.Servers.NatNegotiation.Entity.Structure.Request;
using UniSpyServer.Servers.NatNegotiation.Handler.CmdHandler;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.NatNegotiation.Handler
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
                case RequestType.Init:
                    return new InitHandler(_client, new InitRequest((byte[])rawRequest));
                case RequestType.AddressCheck:
                    return new AddressCheckHandler(_client, new AddressCheckRequest((byte[])rawRequest));
                case RequestType.NatifyRequest:
                    return new NatifyHandler(_client, new NatifyRequest((byte[])rawRequest));
                case RequestType.ErtAck:
                    return new ErtAckHandler(_client, new ErtAckRequest((byte[])rawRequest));
                case RequestType.Report:
                    return new ReportHandler(_client, new ReportRequest((byte[])rawRequest));
                default:
                    return null;
            }
        }
    }
}
