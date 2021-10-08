using QueryReport.Abstraction.BaseClass;
using QueryReport.Entity.Enumerate;
using System.Net;
using UniSpyLib.Extensions;

namespace QueryReport.Entity.Structure.Result
{
    internal sealed class HeartBeatResult : ResultBase
    {
        public EndPoint RemoteEndPoint { private get; set; }
        public string RemoteIP => HtonsExtensions.EndPointToIP(RemoteEndPoint);
        public string RemotePort => HtonsExtensions.EndPointToPort(RemoteEndPoint);
        public HeartBeatResult()
        {
            PacketType = Enumerate.PacketType.HeartBeat;
        }
    }
}
