using UniSpy.Server.Core.Logging;
using UniSpy.Server.QueryReport.Aggregate.Redis.GameServer;
using UniSpy.Server.ServerBrowser.Handler.CmdHandler.AdHoc;

namespace UniSpy.Server.ServerBrowser.Aggregate
{
    public sealed class HeartbeatChannel : QueryReport.Aggregate.Redis.HeartbeatChannel
    {
        /// <summary>
        /// we do not run subscribe() in QR because QR only need to push
        /// We run subscribe() in SB, because SB need to receive message
        /// </summary>
        public override void ReceivedMessage(GameServerInfo message)
        {
            LogWriter.LogInfo($"Received game server message from QR:{message.ServerID}");
            var handler = new AdHocHandler(message);
            handler.Handle();
        }
    }
}