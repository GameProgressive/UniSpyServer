using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Logging;
using QueryReport.Entity.Enumerate;
using QueryReport.Handler.CommandHandler.Available;
using QueryReport.Handler.CommandHandler.Challenge;
using QueryReport.Handler.CommandHandler.ClientMessage;
using QueryReport.Handler.CommandHandler.Echo;
using QueryReport.Handler.CommandHandler.HeartBeat;
using QueryReport.Handler.CommandHandler.KeepAlive;
using QueryReport.Abstraction.BaseClass;

namespace QueryReport.Handler.CommandSwitcher
{
    public class QRCommandSwitcher
    {
        public static void Switch(ISession session, byte[] rawRequest)
        {
            var serializer = new QRRequestSerializer(session, rawRequest);
            serializer.Serialize();

            foreach (QRRequestBase request in serializer.Requests)
            {
                switch (request.PacketType)
                {
                    case QRPacketType.AvaliableCheck:
                        new AvailableHandler(session, request).Handle();
                        break;
                    //verify challenge to check game server is real or fake;
                    //after verify we can add game server to server list
                    case QRPacketType.Challenge:
                        new ChallengeHandler(session, request).Handle();
                        break;
                    case QRPacketType.HeartBeat:
                        new HeartBeatHandler(session, request).Handle();
                        break;
                    case QRPacketType.KeepAlive:
                        new KeepAliveHandler(session, request).Handle();
                        break;
                    case QRPacketType.EchoResponse:
                        new EchoHandler(session, request).Handle();
                        break;
                    case QRPacketType.ClientMessageACK:
                        new ClientMessageACKHandler(session, request).Handle();
                        break;
                    default:
                        LogWriter.UnknownDataRecieved(rawRequest);
                        break;
                }
            }
        }
    }
}
