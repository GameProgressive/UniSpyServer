using System.Collections.Generic;
using UniSpy.Server.NatNegotiation.Enumerate;
using UniSpy.Server.NatNegotiation.Contract.Request;
using UniSpy.Server.NatNegotiation.Handler.CmdHandler;
using UniSpy.Server.Core.Abstraction.BaseClass;
using UniSpy.Server.Core.Abstraction.Interface;

namespace UniSpy.Server.NatNegotiation.Handler
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
            var req = (byte[])rawRequest;
            switch ((RequestType)name)
            {
                case RequestType.Init:
                    return new InitHandler(_client, new InitRequest(req));
                case RequestType.AddressCheck:
                    return new AddressCheckHandler(_client, new AddressCheckRequest(req));
                case RequestType.NatifyRequest:
                    return new NatifyHandler(_client, new NatifyRequest(req));
                case RequestType.ErtAck:
                    return new ErtAckHandler(_client, new ErtAckRequest(req));
                case RequestType.Report:
                    return new ReportHandler(_client, new ReportRequest(req));
                case RequestType.ConnectAck:
                    return new ConnectAckHandler(_client, new ConnectAckRequest(req));
                default:
                    return null;
            }
        }
    }
}
