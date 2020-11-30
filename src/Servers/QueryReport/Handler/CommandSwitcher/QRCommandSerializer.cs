using QueryReport.Abstraction.BaseClass;
using QueryReport.Entity.Enumerate;
using QueryReport.Handler.CommandHandler;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Logging;

namespace QueryReport.Handler.CommandSwitcher
{
    public class QRCommandSerializer : CommandSerializerBase
    {
        protected new byte[] _rawRequest;
        public QRCommandSerializer(ISession session, object rawRequest) : base(session, rawRequest)
        {
            _rawRequest = (byte[])rawRequest;
        }

        public override void Serialize()
        {
            var serializer = new QRRequestSerializer(_session, _rawRequest);
            serializer.Serialize();

            foreach (QRRequestBase request in serializer.Requests)
            {
                switch (request.PacketType)
                {
                    case QRPacketType.AvaliableCheck:
                        new AvailableHandler(_session, request).Handle();
                        break;
                    //verify challenge to check game server is real or fake;
                    //after verify we can add game server to server list
                    case QRPacketType.Challenge:
                        new ChallengeHandler(_session, request).Handle();
                        break;
                    case QRPacketType.HeartBeat:
                        new HeartBeatHandler(_session, request).Handle();
                        break;
                    case QRPacketType.KeepAlive:
                        new KeepAliveHandler(_session, request).Handle();
                        break;
                    case QRPacketType.EchoResponse:
                        new EchoHandler(_session, request).Handle();
                        break;
                    case QRPacketType.ClientMessageACK:
                        new ClientMsgAckHandler(_session, request).Handle();
                        break;
                    default:
                        LogWriter.UnknownDataRecieved(_rawRequest);
                        break;
                }
            }
        }
    }
}
