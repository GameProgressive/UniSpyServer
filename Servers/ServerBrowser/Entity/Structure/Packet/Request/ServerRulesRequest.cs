using System;
using System.Net;
using GameSpyLib.Encryption;

namespace ServerBrowser.Entity.Structure.Packet.Request
{
    public class ServerRulesRequest
    {
        public int IP { get; protected set; }
        public ushort HostPort { get; protected set; }

        public ServerRulesRequest()
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
            
            IP = BitConverter.ToInt32(ByteTools.SubBytes(recv, 3, 4));

            byte[] bytePort = new byte[2];
            ByteTools.SubBytes(recv, 7, 2).CopyTo(bytePort, 0); 

            HostPort = BitConverter.ToUInt16(bytePort);

            return true;
        }
    }
}
