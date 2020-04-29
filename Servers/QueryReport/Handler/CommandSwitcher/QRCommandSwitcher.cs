using GameSpyLib.Common.BaseClass;
using GameSpyLib.Common.Entity.Interface;
using GameSpyLib.Logging;
using QueryReport.Entity.Enumerator;
using QueryReport.Handler.CommandHandler.Available;
using QueryReport.Handler.CommandHandler.Challenge;
using QueryReport.Handler.CommandHandler.Echo;
using QueryReport.Handler.CommandHandler.HeartBeat;
using QueryReport.Handler.CommandHandler.KeepAlive;
using System;

namespace QueryReport.Handler.CommandSwitcher
{
    public class QRCommandSwitcher : CommandSwitcherBase
    {
        public void Switch(ISession client, byte[] recv)
        {
            try
            {
                switch ((QRPacketType)recv[0])
                {
                    case QRPacketType.AvaliableCheck:
                        new AvailableHandler(client, recv).Handle();
                        break;

                    //verify challenge to check game server is real or fake;
                    //after verify we can add game server to server list
                    case QRPacketType.Challenge:
                        new ChallengeHandler(client, recv).Handle();
                        break;

                    case QRPacketType.HeartBeat: // HEARTBEAT
                        new HeartBeatHandler(client, recv).Handle();
                        break;

                    case QRPacketType.KeepAlive:
                        new KeepAliveHandler(client, recv).Handle();
                        break;

                    case QRPacketType.EchoResponse:
                        new EchoHandler(client, recv).Handle();
                        break;

                    default:
                        LogWriter.UnknownDataRecieved(recv);
                        break;
                }
            }
            catch (Exception e)
            {
                LogWriter.ToLog(Serilog.Events.LogEventLevel.Error, e.ToString());
            }
        }
    }
}
