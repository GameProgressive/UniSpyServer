using GameSpyLib.Logging;
using QueryReport.Handler;
using QueryReport.Structure;
using System;
using System.Collections.Generic;
using System.Text;

namespace QueryReport
{
    public class CommandSwitcher
    {
        public static void Switch(QRServer server, byte[] message)
        {
            try
            {
                switch (message[0])
                {
                    case QRClient.Avaliable:
                        AvaliableCheckHandler.BackendAvaliabilityResponse(server, message);
                        break;
                    // Note: BattleSpy make use of this despite not being used in both OpenSpy and the SDK.
                    // Perhaps it was present on an older version of GameSpy SDK
                    case QRGameServer.Challenge:
                        ChallengeHandler.ServerChallengeResponse(server, message);
                        break;
                    case QRClient.Heartbeat: // HEARTBEAT
                        HeartBeatHandler.HeartbeatResponse(server, message);
                        break;
                    case QRClient.KeepAlive:
                        KeepAliveHandler.KeepAliveResponse(server, message);
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
