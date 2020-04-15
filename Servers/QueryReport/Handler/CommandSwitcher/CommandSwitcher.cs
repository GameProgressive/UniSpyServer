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
                        new AvailableHandler().Handle(server, endPoint, recv);
                        break;

                    //verify challenge to check game server is real or fake;
                    //after verify we can add game server to server list
                    case QRPacketType.Challenge:
                        new ChallengeHandler().Handle(server, endPoint, recv);
                        break;

                    case QRPacketType.HeartBeat: // HEARTBEAT
                        new HeartBeatHandler().Handle(server, endPoint, recv);
                        break;

                    case QRPacketType.KeepAlive:
                        new KeepAliveHandler().Handle(server, endPoint, recv);
                        break;

                    case QRPacketType.EchoResponse:
                        new EchoHandler().Handle(server, endPoint, recv);
                        break;

                    default:
                        LogWriter.UnknownDataRecieved(recv);
                        break;
                }
            }
            catch (Exception e)
            {
               LogWriter.ToLog(Serilog.Events.LogEventLevel.Error, e.ToString());
                server.ReceiveAsync();
            }
        }
    }
}
