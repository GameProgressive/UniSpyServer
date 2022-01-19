using UniSpyServer.Servers.QueryReport.Abstraction.BaseClass;
using UniSpyServer.Servers.QueryReport.Entity.Enumerate;
using System.Net;
using UniSpyServer.UniSpyLib.Extensions;

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
