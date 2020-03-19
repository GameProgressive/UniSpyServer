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

        private ServerRulesPacket _packet;

        public ServerRulesHandler(SBSession session, byte[] recv) : base(session, recv)
        {
            _packet = new ServerRulesPacket();
        }
        
        public override void CheckRequest(SBSession session, byte[] recv)
        {
            base.CheckRequest(session, recv);

            if (_packet.Parse(recv))

            {
                _errorCode = Entity.Enumerator.SBErrorCode.Parse;
                return;
            }
        }

        public override void DataOperation(SBSession session, byte[] recv)
        {
            base.DataOperation(session, recv);
            var server = QueryReport.Server.QRServer.GameServerList.
              Where(c => c.Value.PublicIP==_packet.IP&&c.Value.ServerData.StandardKeyValue["hostport"]==_packet.HostPort);

            //string port = Convert.ToString(BitConverter.ToUInt16(_rulesPacket.Port))
            ////get server here
            //var result = QueryReport.Server.QRServer.GameServerList.
            //  Where(c => c.Value.PublicIP == _rulesPacket.IP
            //  && c.Value.ServerInfo.Data["hostport"]==Convert.ToString(BitConverter.ToUInt16(_rulesPacket.Port)));
        }

        public override void ConstructResponse(SBSession session, byte[] recv)
        {
        }
    }
}
