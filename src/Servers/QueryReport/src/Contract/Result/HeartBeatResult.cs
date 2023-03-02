using UniSpy.Server.QueryReport.Abstraction.BaseClass;
using System.Net;

namespace UniSpy.Server.QueryReport.Contract.Result
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
