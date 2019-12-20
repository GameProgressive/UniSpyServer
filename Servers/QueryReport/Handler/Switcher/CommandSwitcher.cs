using GameSpyLib.Logging;
using QueryReport.Handler.Available;
using QueryReport.Handler.Challenge;
using QueryReport.Handler.HeartBeat;
using QueryReport.Handler.KeepAlive;
using QueryReport.Server;
using QueryReport.Structure;
using System;
using System.Net;

namespace QueryReport.Handler
{
    public class CommandSwitcher
    {
        public static void Switch(QRServer server, EndPoint endPoint, byte[] message)
        {
            try
            {
                switch (message[0])
                {
                    case QRClient.Avaliable:
                        AvailableCheckHandler.BackendAvaliabilityResponse(server, endPoint, message);
                        break;
                    // Note: BattleSpy make use of this despite not being used in both OpenSpy and the SDK.
                    // Perhaps it was present on an older version of GameSpy SDK
                    case QRGameServer.Challenge:
                        ChallengeHandler.ServerChallengeResponse(server, endPoint, message);
                        break;
                    case QRClient.Heartbeat: // HEARTBEAT
                        HeartBeatHandler.HeartbeatResponse(server, endPoint, message);
                        break;
                    case QRClient.KeepAlive:
                        KeepAliveHandler.KeepAliveResponse(server, endPoint, message);
                        break;
                    default:
                        server.UnknownDataRecived(message);
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
