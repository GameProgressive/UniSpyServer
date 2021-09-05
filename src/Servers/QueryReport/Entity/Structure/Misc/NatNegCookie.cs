using System.Net;

namespace QueryReport.Entity.Structure.NATNeg
{
    public class NATNegCookie
    {
        public string GameServerRemoteIP { get; set; }
        public string GameServerRemotePort { get; set; }
        public IPEndPoint GameServerRemoteEndPoint { get; set; }
        public string GameName { get; set; }
        public byte[] NatNegMessage { get; set; }

        public NATNegCookie()
        {
        }
    }
}
