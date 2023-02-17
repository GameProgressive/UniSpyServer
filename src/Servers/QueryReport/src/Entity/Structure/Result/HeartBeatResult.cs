using UniSpy.Server.QueryReport.V2.Abstraction.BaseClass;
using System.Net;

namespace UniSpy.Server.QueryReport.V2.Entity.Structure.Result
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
