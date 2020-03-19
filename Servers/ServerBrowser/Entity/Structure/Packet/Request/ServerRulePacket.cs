using System;
using GameSpyLib.Encryption;

namespace ServerBrowser.Entity.Structure.Packet.Request
{
    public class ServerRulesPacket
    {
        public byte[] IP { get; protected set; }
        public string HostPort { get; protected set; }

        public ServerRulesPacket()
        {
        }

        public bool Parse(byte[] recv)
        {
            byte[] byteLength = ByteTools.SubBytes(recv, 0, 2);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(byteLength);
            }
            if (BitConverter.ToInt16(byteLength) != recv.Length)
            {
                return false;
            }
            ByteTools.SubBytes(recv, 2, 4).CopyTo(IP, 0);

            byte[] bytePort = new byte[2];
            ByteTools.SubBytes(recv, 7, 2).CopyTo(bytePort, 0);

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytePort);
            }

            HostPort = BitConverter.ToUInt16(bytePort).ToString();

            return true;
        }
    }
}
