using System.Linq;
using ServerBrowser.Entity.Structure.Packet.Request;

namespace ServerBrowser.Handler.CommandHandler.ServerInfo
{
    /// <summary>
    /// Get full rules for a server (for example, to get
    /// player information from a server that only has basic information so far)
    /// </summary>
    public class ServerRulesHandler : CommandHandlerBase
    {

        private ServerRulesRequest _request;

        public ServerRulesHandler(SBSession session, byte[] recv) : base(session, recv)
        {
        }
        
        public override void CheckRequest(SBSession session, byte[] recv)
        {
            base.CheckRequest(session, recv);
            _request = new ServerRulesRequest();
            if (!_request.Parse(recv))
            {
                _errorCode = Entity.Enumerator.SBErrorCode.Parse;
                return;
            }
        }

        public override void DataOperation(SBSession session, byte[] recv)
        {
            base.DataOperation(session, recv);
            var server = QueryReport.Server.QRServer.GameServerList.
              Where(c => c.Value.PublicIP==_request.IP);
        }

        public override void ConstructResponse(SBSession session, byte[] recv)
        {
        }
    }
}
