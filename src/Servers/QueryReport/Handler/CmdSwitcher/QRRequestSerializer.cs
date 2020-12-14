using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Logging;
using QueryReport.Abstraction.BaseClass;
using QueryReport.Entity.Enumerate;
using QueryReport.Entity.Structure.Request;
using UniSpyLib.Abstraction.BaseClass;

namespace QueryReport.Handler.CmdSwitcher
{
    public class QRRequestSerializer:UniSpyRequestSerializerBase
    {
        protected new byte[] _rawRequest
        {
            get { return (byte[])base._rawRequest; }
            set { base._rawRequest = value; }
        }

        public QRRequestSerializer(byte[] rawRequest) : base(rawRequest)
        {
            _rawRequest = rawRequest;
        }

        public override IUniSpyRequest Serialize()
        {
            if (_rawRequest.Length < 1)
            {
                return null;
            }

            switch ((QRPacketType)_rawRequest[0])
            {
                case QRPacketType.AvaliableCheck:
                    return new AvaliableRequest(_rawRequest);
                //verify challenge to check game server is real or fake;
                //after verify we can add game server to server list
                case QRPacketType.Challenge:
                    return new ChallengeRequest(_rawRequest);
                case QRPacketType.HeartBeat:
                    return new HeartBeatRequest(_rawRequest);

                case QRPacketType.KeepAlive:
                    return new KeepAliveRequest(_rawRequest);

                case QRPacketType.EchoResponse:
                    return new QRRequestBase(_rawRequest);

                case QRPacketType.ClientMessageACK:
                    return new QRRequestBase(_rawRequest);

                default:
                    LogWriter.UnknownDataRecieved(_rawRequest);
                    return null;
            }
        }
    }
}
