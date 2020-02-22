using GameSpyLib.Logging;
using QueryReport.Entity.Structure;
using QueryReport.Handler.CommandHandler.Available;
using QueryReport.Handler.CommandHandler.Challenge;
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
                switch (recv[0])
                {
                    case QRClient.Avaliable:
                        AvailableHandler available = new AvailableHandler(server, endPoint, recv);
                        break;
                    // Note: BattleSpy make use of this despite not being used in both OpenSpy and the SDK.
                    // Perhaps it was present on an older version of GameSpy SDK
                    case QRGameServer.Challenge:
                        ChallengeHandler.ServerChallengeResponse(server, endPoint, recv);
                        break;
                    case QRClient.Heartbeat: // HEARTBEAT
                        HeartBeatHandler.HeartbeatResponse(server, endPoint, recv);
                        break;
                    case QRClient.KeepAlive:
                        KeepAliveHandler.KeepAliveResponse(server, endPoint, recv);
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
