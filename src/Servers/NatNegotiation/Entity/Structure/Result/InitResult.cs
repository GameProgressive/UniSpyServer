using System.Net;
using NATNegotiation.Abstraction.BaseClass;
using NATNegotiation.Entity.Enumerate;

namespace NATNegotiation.Entity.Structure.Result
{
    public class InitResult : NNResultBase
    {
        public string LocalIP => ((IPEndPoint)LocalEndPoint).Address.ToString();
        public ushort LocalPort => (ushort)((IPEndPoint)LocalEndPoint).Port;
        public EndPoint LocalEndPoint { get; set; }
        public InitResult()
        {
            PacketType = NatPacketType.InitAck;
        }
    }
}
