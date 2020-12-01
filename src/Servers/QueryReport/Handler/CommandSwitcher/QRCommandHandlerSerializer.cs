using QueryReport.Abstraction.BaseClass;
using QueryReport.Entity.Enumerate;
using QueryReport.Handler.CommandHandler;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Logging;

namespace QueryReport.Handler.CommandSwitcher
{
    public class QRCommandHandlerSerializer : CommandHandlerSerializerBase
    {
        protected new QRRequestBase _request;
        public QRCommandHandlerSerializer(ISession session, IRequest request) : base(session, request)
        {
            _request = (QRRequestBase)request;
        }

        public override IHandler Serialize()
        {
            switch (_request.PacketType)
            {
                case QRPacketType.AvaliableCheck:
                    return new AvailableHandler(_session, _request);

                //verify challenge to check game server is real or fake;
                //after verify we can add game server to server list
                case QRPacketType.Challenge:
                    return new ChallengeHandler(_session, _request);

                case QRPacketType.HeartBeat:
                    return new HeartBeatHandler(_session, _request);

                case QRPacketType.KeepAlive:
                    return new KeepAliveHandler(_session, _request);

                case QRPacketType.EchoResponse:
                    return new EchoHandler(_session, _request);

                case QRPacketType.ClientMessageACK:
                    return new ClientMsgAckHandler(_session, _request);

                default:
                    LogWriter.UnknownDataRecieved(_request.RawRequest);
                    return null;

            }
        }
    }
}
