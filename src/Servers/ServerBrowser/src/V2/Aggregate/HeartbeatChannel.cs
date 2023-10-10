using UniSpy.Server.Core.Logging;
using UniSpy.Server.QueryReport.V2.Aggregate.Redis.GameServer;
using UniSpy.Server.ServerBrowser.V2.Handler.CmdHandler.AdHoc;

namespace UniSpy.Server.ServerBrowser.V2.Aggregate
{
    public sealed class HeartbeatChannel : QueryReport.V2.Aggregate.Redis.HeartbeatChannel
    {
        /// <summary>
        /// we do not run subscribe() in QR because QR only need to push
        /// We run subscribe() in SB, because SB need to receive message
        /// </summary>
        public override void ReceivedMessage(GameServerCache message)
        {
            LogWriter.LogInfo($"Received game server message from QR:{message.ServerID}");
            var handler = new AdHocHandler(message);
            handler.Handle();
        }
    }
}