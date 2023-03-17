using System.Collections.Generic;
using UniSpy.Server.Core.Abstraction.BaseClass;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.QueryReport.Enumerate;
using UniSpy.Server.QueryReport.Handler.CmdHandler;

namespace UniSpy.Server.QueryReport.Handler
{
    public sealed class CmdSwitcher : CmdSwitcherBase
    {
        private new byte[] _rawRequest => (byte[])base._rawRequest;
        public CmdSwitcher(IClient client, object rawRequest) : base(client, rawRequest)
        {
        }
        protected override void ProcessRawRequest()
        {
            // var rawRequest = base._rawRequest as byte[];
            if (_rawRequest.Length < 4)
            {
                throw new UniSpyException("Invalid request");
            }

            // qr v2 protocol
            var name = (RequestType)_rawRequest[0];
            var rawRequest = _rawRequest;
            _requests.Add(new KeyValuePair<object, object>(name, rawRequest));
        }

        protected override IHandler CreateCmdHandlers(object name, object rawRequest)
        {
            var req = (byte[])rawRequest;
            // query report v2
            switch ((RequestType)name)
            {
                case RequestType.HeartBeat:
                    return new HeartBeatHandler(_client, new Contract.Request.HeartBeatRequest(req));
                case RequestType.Challenge:
                    return new ChallengeHandler(_client, new Contract.Request.ChallengeRequest(req));
                case RequestType.AvaliableCheck:
                    return new AvailableHandler(_client, new Contract.Request.AvaliableRequest(req));
                case RequestType.ClientMessageAck:
                    return new ClientMessageAckHandler(_client, new Contract.Request.ClientMessageAckRequest(req));
                case RequestType.Echo:
                    return new EchoHandler(_client, new Contract.Request.EchoRequest(req));
                case RequestType.KeepAlive:
                    return new KeepAliveHandler(_client, new Contract.Request.KeepAliveRequest(req));
                default:
                    return null;
            }
        }
    }
}
