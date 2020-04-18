using GameSpyLib.Extensions;
using System.Net;

namespace ServerBrowser.Entity.Structure.Packet.Request
{
    public class AdHocRequest
    {
        /// <summary>
        /// The game server client search for
        /// </summary>
        public string TargetServerIP { get; protected set; }
        public string TargetServerHostPort { get; protected set; }

        public AdHocRequest()
        {
        }

        public bool Parse(byte[] recv)
        {
            ushort length = ByteTools.ToUInt16(ByteTools.SubBytes(recv, 0, 2), true);

            if (length != recv.Length)
            {
                return false;
            }

            byte[] ip = ByteTools.SubBytes(recv, 3, 4);
            byte[] port = ByteTools.SubBytes(recv, 7, 2);

            IPEndPoint iPEnd = ByteTools.GetIPEndPoint(ip, port);

            TargetServerIP = iPEnd.Address.ToString();
            TargetServerHostPort = iPEnd.Port.ToString();

            return true;
        }
    }
}
