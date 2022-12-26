using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniSpyServer.Servers.QueryReport.Entity.Enumerate;
using UniSpyServer.Servers.QueryReport.Entity.Structure.Request;
using UniSpyServer.Servers.QueryReport.Handler.CmdHandler;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Encryption;

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
            // var rawRequest = base._rawRequest as byte[];
            if (_rawRequest.Length < 4)
            {
                throw new UniSpyException("Invalid request");
            }
            object name;
            object rawRequest;
            if (_rawRequest[0] == '\\')
            {
                // qr v1 protocol
                rawRequest = Encoding.ASCII.GetString(_rawRequest);
                var cmdFrags = ((string)rawRequest).Split('\\');
                name = cmdFrags[0];
            }
            else
            {
                // qr v2 protocol
                name = (RequestType)_rawRequest[0];
                rawRequest = _rawRequest;
            }
            _requests.Add(new KeyValuePair<object, object>(name, rawRequest));
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
                // query report v2
                switch ((RequestType)name)
                {
                    case RequestType.HeartBeat:
                        return new HeartBeatHandler(_client, new HeartBeatRequest(req));
                    case RequestType.Challenge:
                        return new ChallengeHandler(_client, new ChallengeRequest(req));
                    case RequestType.AvaliableCheck:
                        return new AvailableHandler(_client, new AvaliableRequest(req));
                    case RequestType.ClientMessageAck:
                        return new ClientMessageAckHandler(_client, new ClientMessageAckRequest(req));
                    case RequestType.Echo:
                        return new EchoHandler(_client, new EchoRequest(req));
                    case RequestType.KeepAlive:
                        return new KeepAliveHandler(_client, new KeepAliveRequest(req));
                    default:
                        return null;
                }
            }
        }
    }
}
