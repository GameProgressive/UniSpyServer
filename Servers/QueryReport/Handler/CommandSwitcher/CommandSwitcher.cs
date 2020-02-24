using GameSpyLib.Logging;
using QueryReport.Entity.Enumerator;
using QueryReport.Handler.CommandHandler.Available;
using QueryReport.Handler.CommandHandler.Echo;
using QueryReport.Handler.CommandHandler.HeartBeat;
using QueryReport.Handler.CommandHandler.KeepAlive;
using QueryReport.Server;
using System;
using System.Net;

namespace QueryReport.Handler.CommandSwitcher
{
    public class CommandSwitcher
    {
        public static void Switch(QRServer server, EndPoint endPoint, byte[] recv)
        {
            try
            {
                switch ((QRPacketType)recv[0])
                {
                    case QRPacketType.AvaliableCheck:
                        AvailableHandler available = new AvailableHandler(server, endPoint, recv);
                        break;
                    // Note: BattleSpy make use of this despite not being used in both OpenSpy and the SDK.
                    // Perhaps it was present on an older version of GameSpy SDK
                    //case QRRequestPacket.Challenge:
                    //    ChallengeHandler.ServerChallengeResponse(server, endPoint, recv);
                    //   break;
                    case QRPacketType.HeartBeat: // HEARTBEAT
                        HeartBeatHandler heart = new HeartBeatHandler(server, endPoint, recv);
                        break;
                    case QRPacketType.KeepAlive:
                        KeepAliveHandler keep = new KeepAliveHandler(server, endPoint, recv);
                        break;
                    case QRPacketType.EchoResponse:
                        EchoHandler echo = new EchoHandler(server, endPoint, recv);
                        break;
                    default:
                        server.UnknownDataRecived(recv);
                        break;
                }
            }
            catch (Exception e)
            {
                LogWriter.Log.WriteException(e);
                server.ReceiveAsync();
            }
        }
    }
}
