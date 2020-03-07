using ServerBrowser.Entity.Structure.Packet.Request;

namespace ServerBrowser.Handler.CommandHandler.ServerInfo
{
    public class ServerInfoHandler:SBHandlerBase
    {
        public ServerInfoHandler(SBSession session, byte[] recv) : base(session, recv)
        {
        }
        private ServerInfoPacket _infoPacket;
        public override void CheckRequest(SBSession session, byte[] recv)
        {
            base.CheckRequest(session, recv);
            if (recv.Length > 17)
            {
                _errorCode = Entity.Enumerator.SBErrorCode.Parse;
                return;
            }
            _infoPacket = new ServerInfoPacket(recv);
        }

        public override void DataOperation(SBSession session, byte[] recv)
        {
            base.DataOperation(session, recv);
        }

        public override void ConstructResponse(SBSession session, byte[] recv)
        {
            base.ConstructResponse(session, recv);
        }

    }
}
