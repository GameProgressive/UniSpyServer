using System;
using System.Linq;
using System.Text;

namespace ServerBrowser.Entity.Structure
{
    public class ServerListPacket
    {
        public byte QueryLenth { get; protected set; }
        public byte RequestVersion { get; protected set; }
        public byte ProtocolVersion { get; protected set; }
        public byte EncodingVersion { get; protected set; }
        public byte GameVersion { get; protected set; }


        private string _queryForGameName;
        private string _queryFromGameName;
        private string _challenge;

        public string[] DataField { get; protected set; }
        public byte[] Filter;

        public ServerListPacket(byte[] recv)
        {
            QueryLenth = recv[1];
            RequestVersion = recv[2];
            ProtocolVersion = recv[3];
            EncodingVersion = recv[4];
            GameVersion = recv[5];
            string tempStr = Encoding.ASCII.GetString(recv.Skip(9).ToArray());
            string[] dataFrag = tempStr.Split('\0', StringSplitOptions.RemoveEmptyEntries);
            _queryForGameName = dataFrag[0];
            _queryFromGameName = dataFrag[1];
            _challenge = dataFrag[2];
            DataField = dataFrag[3].Split('\\', StringSplitOptions.RemoveEmptyEntries);   
        }


        public byte[] QueryForGameName
        {
            get
            { return Encoding.ASCII.GetBytes(_queryForGameName); }
        }

        public byte[] QueryFromGameName
        {
            get
            {
                return  Encoding.ASCII.GetBytes(_queryFromGameName);
            }
        }
        public byte[] Challenge
        {
            get
            { return Encoding.ASCII.GetBytes(_challenge); }
        }


    }
}
