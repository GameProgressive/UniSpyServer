using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.NatNegotiation.Application;
using UniSpy.Server.NatNegotiation.Contract.Request;
using UniSpy.Server.NatNegotiation.Enumerate;
using UniSpy.Server.NatNegotiation.Handler.CmdHandler;

namespace UniSpy.Server.NatNegotiation.Aggregate.GameTrafficRelay
{
    public class RelaySwitcher : Handler.CmdSwitcher
    {
        public RelaySwitcher(Client client, byte[] rawRequest) : base(client, rawRequest)
        {
        }
        protected override IHandler CreateCmdHandlers(object name, object rawRequest)
        {
            var req = (byte[])rawRequest;
            switch ((Enumerate.RequestType)name)
            {
                case RequestType.Ping:
                    return new PingHandler(_client, new PingRequest(req));
                default:
                    throw new NatNegotiation.Exception("GameTrafficRelay only process ping request");
            }
        }
    }
}