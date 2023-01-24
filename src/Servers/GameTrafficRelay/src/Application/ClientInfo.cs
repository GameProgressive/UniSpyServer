using UniSpyServer.Servers.NatNegotiation.Entity.Enumerate;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;

namespace UniSpyServer.Servers.GameTrafficRelay.Application
{
    public sealed class ClientInfo : ClientInfoBase
    {
        /// <summary>
        /// The other client that we transit message to
        /// </summary>
        /// <value></value>
        public Client TrafficRelayTarget { get; set; }
        public byte[] PingData { get; set; }
        public uint? Cookie { get; set; }
        public NatClientIndex? ClientIndex { get; set; }
        public ClientInfo() : base()
        {
        }
    }
}