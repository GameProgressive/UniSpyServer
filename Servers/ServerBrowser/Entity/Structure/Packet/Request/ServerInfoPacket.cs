using System;
using GameSpyLib.Encryption;

namespace ServerBrowser.Entity.Structure.Packet.Request
{
    public class ServerRulesPacket
    {
        public string IP { get; protected set; }

        public string Port { get; protected set; }

        public ServerRulesPacket(byte[] recv)
        {
            IP = string.Format($"{recv[2]}.{recv[3]}.{recv[4]}.{recv[5]}");

            ushort port;

            port = BitConverter.ToUInt16(ByteTools.SubBytes(recv, 7, 2));
            byte[] temp = new byte[2];
            ByteTools.SubBytes(recv, 7, 2).CopyTo(temp, 0);

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(temp);
            }

            port = BitConverter.ToUInt16(temp);
        }
    }
}
