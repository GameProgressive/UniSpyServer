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
        public QRRequestSerializer(ISession session, byte[] rawRequest) : base(session, rawRequest)
        {
            _rawRequest = rawRequest;
        }

        public override void Serialize()
        {
            if (_rawRequest.Length < 1)
            {
                return;
            }

            var request = GenerateRequest(_rawRequest);

            if (request == null)
            {
                return;
            }

            if (!(bool)request.Parse())
            {
                LogWriter.ToLog(LogEventLevel.Error, ErrorMessage.GetErrorMessage(QRErrorCode.Parse));
                return;
            }

            Requests.Add(request);
        }

        protected override IRequest GenerateRequest(object singleRequest)
        {
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
