using UniSpyServer.Servers.QueryReport.Abstraction.BaseClass;
using System.Net;

namespace UniSpyServer.Servers.QueryReport.Entity.Structure.Result
{
    public sealed class HeartBeatResult : ResultBase
    {
        public IPEndPoint RemoteIPEndPoint { get; set; }
        public HeartBeatResult()
        {
            PacketType = Enumerate.PacketType.HeartBeat;
        }
    }
}
