using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Logging;
using QueryReport.Entity.Structure;
using System;

namespace QueryReport.Handler.SystemHandler.ServerList
{
    public class ServerListManager : UniSpyTimerBase
    {

        protected override void CheckExpire()
        {
            base.CheckExpire();
            var servers = GameServer.GetAllServersWithKeys();
            foreach (var server in servers)
            {
                // we calculate the interval between last packe and current time
                var duration = DateTime.Now.Subtract(server.Value.LastPacket).TotalSeconds;
                if (duration > 120)
                {
                    GameServer.DeleteSpecificServer(server.Key);
                    LogWriter.ToLog($"Delete expired game server :{server.Key}");
                }
            }
        }
    }
}
