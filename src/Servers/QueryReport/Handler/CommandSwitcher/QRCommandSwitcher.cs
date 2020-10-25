using GameSpyLib.Common.Entity.Interface;
using GameSpyLib.Logging;
using QueryReport.Entity.Enumerator;
using QueryReport.Handler.CommandHandler.Available;
using QueryReport.Handler.CommandHandler.Challenge;
using QueryReport.Handler.CommandHandler.ClientMessage;
using QueryReport.Handler.CommandHandler.Echo;
using QueryReport.Handler.CommandHandler.HeartBeat;
using QueryReport.Handler.CommandHandler.KeepAlive;
using Serilog.Events;
using System;

namespace QueryReport.Handler.CommandSwitcher
{
    public class QRCommandSwitcher
    {
        public static void Switch(ISession session, byte[] recv)
        {
            try
            {
                switch ((QRPacketType)recv[0])
                {
                    case QRPacketType.AvaliableCheck:
                        new AvailableHandler(session, recv).Handle();
                        break;
                    //verify challenge to check game server is real or fake;
                    //after verify we can add game server to server list
                    case QRPacketType.Challenge:
                        new ChallengeHandler(session, recv).Handle();
                        break;
                    case QRPacketType.HeartBeat:
                        new HeartBeatHandler(session, recv).Handle();
                        break;
                    case QRPacketType.KeepAlive:
                        new KeepAliveHandler(session, recv).Handle();
                        break;
                    case QRPacketType.EchoResponse:
                        new EchoHandler(session, recv).Handle();
                        break;
                    case QRPacketType.ClientMessageACK:
                        new ClientMessageACKHandler(session, recv).Handle();
                        break;
                    default:
                        LogWriter.UnknownDataRecieved(recv);
                        break;
                }
            }
            catch (Exception e)
            {
                LogWriter.ToLog(LogEventLevel.Error, e.ToString());
            }
        }
    }
}
