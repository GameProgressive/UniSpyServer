using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniSpy.Server.Core.Abstraction.BaseClass;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Core.Encryption;
using UniSpy.Server.Core.Logging;

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
            object name;
            object rawRequest;
            if (_rawRequest[0] == '\\')
            {
                // qr v1 protocol
                rawRequest = Encoding.ASCII.GetString(_rawRequest);
                var cmdFrags = ((string)rawRequest).Split('\\');
                name = cmdFrags[0];
                _requests.Add(new KeyValuePair<object, object>(name, rawRequest));
            }
            else
            {
                // qr v2 protocol
                name = (V2.Enumerate.RequestType)_rawRequest[0];
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
                switch ((string)name)
                {
                    case "heartbeat":
                    case "echo":
                    case "validate":
                    default:
                        return null;
                }
            }
            // else it is protocol v2
            else
            {
                var req = (byte[])rawRequest;
                // query report v2
                switch ((V2.Enumerate.RequestType)name)
                {
                    case V2.Enumerate.RequestType.HeartBeat:
                        return new V2.Handler.CmdHandler.HeartBeatHandler(_client, new V2.Contract.Request.HeartBeatRequest(req));
                    case V2.Enumerate.RequestType.Challenge:
                        return new V2.Handler.CmdHandler.ChallengeHandler(_client, new V2.Contract.Request.ChallengeRequest(req));
                    case V2.Enumerate.RequestType.AvaliableCheck:
                        return new V2.Handler.CmdHandler.AvailableHandler(_client, new V2.Contract.Request.AvaliableRequest(req));
                    case V2.Enumerate.RequestType.ClientMessageAck:
                        return new V2.Handler.CmdHandler.ClientMessageAckHandler(_client, new V2.Contract.Request.ClientMessageAckRequest(req));
                    case V2.Enumerate.RequestType.Echo:
                        return new V2.Handler.CmdHandler.EchoHandler(_client, new V2.Contract.Request.EchoRequest(req));
                    case V2.Enumerate.RequestType.KeepAlive:
                        return new V2.Handler.CmdHandler.KeepAliveHandler(_client, new V2.Contract.Request.KeepAliveRequest(req));
                    default:
                        return null;
                }
            }
        }
    }
}
