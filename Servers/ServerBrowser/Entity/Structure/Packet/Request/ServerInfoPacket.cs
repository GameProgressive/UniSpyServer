using System;
using GameSpyLib.Encryption;

namespace ServerBrowser.Entity.Structure.Packet.Request
{
    public class ServerInfoPacket
    {
        public byte[] IP { get; protected set; }
        public byte[] Port { get; protected set; }
        

        public ServerInfoPacket(byte[] recv)
        {
            IP = new byte[4];
            Port = new byte[2];

            ByteTools.SubBytes(recv, 2, 4).CopyTo(IP, 0);
            ByteTools.SubBytes(recv, 7, 2).CopyTo(Port, 0);
        }
    }
}
