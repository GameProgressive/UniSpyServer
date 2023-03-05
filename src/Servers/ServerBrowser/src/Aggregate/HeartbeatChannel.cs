using System.Threading.Tasks;
using UniSpy.Server.QueryReport.Contract.Request;
using UniSpy.Server.ServerBrowser.Application;
using UniSpy.Server.ServerBrowser.Handler.CmdHandler.AdHoc;

namespace UniSpy.Server.ServerBrowser.Aggregate
{
    public sealed class HeartbeatChannel : QueryReport.Aggregate.Redis.HeartbeatChannel
    {
        /// <summary>
        /// we do not run subscribe() in QR because QR only need to push
        /// We run subscribe() in SB, because SB need to receive message
        /// </summary>
        public override void ReceivedMessage(HeartBeatRequest message)
        {
            var handler = new AdHocHandler(message);
            handler.Handle();
        }
    }
}