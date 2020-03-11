using System;
using System.Collections.Generic;
using GameSpyLib.Encryption;
using ServerBrowser.Entity.Structure.Packet.Request;

namespace ServerBrowser.Handler.CommandHandler.ServerInfo
{
    /// <summary>
    /// Get full information (for example, to get full rules and
    /// player information from a server that only has basic information so far)
    /// </summary>
    public class ServerInfoHandler : SBHandlerBase
    {
        public ServerInfoHandler(SBSession session, byte[] recv) : base(session, recv)
        {
        }
        private ServerInfoPacket _infoPacket;
        public override void CheckRequest(SBSession session, byte[] recv)
        {
            base.CheckRequest(session, recv);
            byte[] lengthByte = new byte[2];
            ByteTools.SubBytes(recv, 0, 2).CopyTo(lengthByte, 0);

            if (BitConverter.IsLittleEndian)
                Array.Reverse(lengthByte);
            if (BitConverter.ToUInt16(lengthByte, 0) != recv.Length||recv.Length>17)
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
           
            
            
        }

    }
}
