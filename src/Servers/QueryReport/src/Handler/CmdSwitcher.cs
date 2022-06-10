using System.Collections.Generic;

using UniSpyServer.Servers.QueryReport.Entity.Enumerate;
using UniSpyServer.Servers.QueryReport.Entity.Structure.Request;
using UniSpyServer.Servers.QueryReport.Handler.CmdHandler;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.QueryReport.Handler
{
    public sealed class CmdSwitcher : CmdSwitcherBase
    {
        private new byte[] _rawRequest => (byte[])base._rawRequest;
        public CmdSwitcher(IClient client, object rawRequest) : base(client, rawRequest)
        {
        }
        protected override void ProcessRawRequest()
        {
            var req = base._rawRequest as byte[];
            if (_rawRequest.Length < 4)
            {
                throw new UniSpyException("Invalid request");
            }
            var name = (RequestType)_rawRequest[0];
            _requests.Add(new KeyValuePair<object, object>(name, _rawRequest));
        }

        protected override IHandler CreateCmdHandlers(object name, object rawRequest)
        {
            switch ((RequestType)name)
            {
                case RequestType.HeartBeat:
                    return new HeartBeatHandler(_client, new HeartBeatRequest((byte[])rawRequest));
                case RequestType.Challenge:
                    return new ChallengeHandler(_client, new ChallengeRequest((byte[])rawRequest));
                case RequestType.AvaliableCheck:
                    return new AvailableHandler(_client, new AvaliableRequest((byte[])rawRequest));
                case RequestType.ClientMessageAck:
                    return new ClientMessageAckHandler(_client, new ClientMessageAckRequest((byte[])rawRequest));
                case RequestType.Echo:
                    return new EchoHandler(_client, new EchoRequest((byte[])rawRequest));
                case RequestType.KeepAlive:
                    return new KeepAliveHandler(_client, new KeepAliveRequest((byte[])rawRequest));
                default:
                    return null;
            }
        }
    }
}
