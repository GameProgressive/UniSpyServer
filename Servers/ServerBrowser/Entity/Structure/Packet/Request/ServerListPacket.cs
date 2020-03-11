using System;
using System.Linq;
using System.Text;

namespace ServerBrowser.Entity.Structure.Packet.Request
{
    /// <summary>
    /// ServerList also called ServerRule
    /// </summary>
    public class ServerListPacket
    {
        public byte QueryLenth { get; protected set; }
        public byte RequestVersion { get; protected set; }
        public byte ProtocolVersion { get; protected set; }
        public byte EncodingVersion { get; protected set; }
        public byte GameVersion { get; protected set; }

        public string DevGameName { get; protected set; }
        public string GameName { get; protected set; }
        public string Challenge { get; protected set; }

        public string[] Keys { get; protected set; }
        public string Filter;

        public ServerListPacket(byte[] recv)
        {
            QueryLenth = recv[1];
            RequestVersion = recv[2];
            ProtocolVersion = recv[3];
            EncodingVersion = recv[4];
            GameVersion = recv[5];
            string tempStr = Encoding.ASCII.GetString(recv.Skip(9).ToArray());
            string[] dataFrag = tempStr.Split('\0', StringSplitOptions.RemoveEmptyEntries);
            DevGameName = dataFrag[0];
            GameName = dataFrag[1];
            Challenge = dataFrag[2].Substring(0, 8);
            if (dataFrag[2].Length > 8)
            {
                Filter = dataFrag[2].Substring(8);
            }
            Keys = dataFrag[3].Split('\\', StringSplitOptions.RemoveEmptyEntries);
        }


       
    }
}
