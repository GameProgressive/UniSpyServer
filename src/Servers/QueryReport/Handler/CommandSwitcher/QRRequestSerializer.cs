using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Logging;
using QueryReport.Abstraction.BaseClass;
using QueryReport.Entity.Enumerate;
using QueryReport.Entity.Structure.Request;
using QueryReport.Handler.SystemHandler.ErrorMessage;
using Serilog.Events;
using UniSpyLib.Abstraction.BaseClass;

namespace QueryReport.Handler.CommandSwitcher
{
    public class QRRequestSerializer:RequestSerializerBase
    {
        protected new byte[] _rawRequest;
        public QRRequestSerializer(byte[] rawRequest) : base(rawRequest)
        {
            _rawRequest = rawRequest;
        }

        public override IRequest Serialize()
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
