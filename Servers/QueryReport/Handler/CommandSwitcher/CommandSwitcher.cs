using GameSpyLib.Logging;
using QueryReport.Entity.Enumerator;
using QueryReport.Handler.CommandHandler.Available;
using QueryReport.Handler.CommandHandler.Challenge;
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
                    //verify challenge to check game server is real or fake;
                    //after verify we can add game server to server list
                    case QRPacketType.Challenge:
                        ChallengeHandler challenge = new ChallengeHandler(server, endPoint, recv);
                        break;
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
