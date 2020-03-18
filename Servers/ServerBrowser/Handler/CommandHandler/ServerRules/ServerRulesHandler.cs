using System;
using System.Collections.Generic;
using System.Linq;
using GameSpyLib.Encryption;
using ServerBrowser.Entity.Structure.Packet.Request;

namespace ServerBrowser.Handler.CommandHandler.ServerInfo
{
    /// <summary>
    /// Get full rules (for example, to get
    /// player information from a server that only has basic information so far)
    /// </summary>
    public class ServerRulesHandler : CommandHandlerBase
    {
        public ServerRulesHandler(SBSession session, byte[] recv) : base(session, recv)
        {
        }
        private ServerRulesPacket _rulesPacket;

        public override void CheckRequest(SBSession session, byte[] recv)
        {
            base.CheckRequest(session, recv);
            byte[] lengthByte = new byte[2];
            ByteTools.SubBytes(recv, 0, 2).CopyTo(lengthByte, 0);

            if (BitConverter.IsLittleEndian)
                Array.Reverse(lengthByte);
            if (BitConverter.ToUInt16(lengthByte, 0) != recv.Length || recv.Length > 17)
            {
                _errorCode = Entity.Enumerator.SBErrorCode.Parse;
                return;
            }
            _rulesPacket = new ServerRulesPacket(recv);
        }

        public override void DataOperation(SBSession session, byte[] recv)
        {
            base.DataOperation(session, recv);
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
