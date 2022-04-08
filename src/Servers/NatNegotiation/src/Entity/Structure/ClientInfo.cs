using System.Net;
using UniSpyServer.Servers.NatNegotiation.Entity.Enumerate;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;

namespace UniSpyServer.Servers.NatNegotiation.Entity.Structure
{
    public class ClientInfo : ClientInfoBase
    {
        // /// <summary>
        // /// whether uses natneg server to redirect message
        // /// </summary>
        // /// <value></value>
        // public bool IsTransitNetowrkTraffic { get; set; }
        // /// <summary>
        // /// The other client that we transit message to
        // /// </summary>
        // /// <value></value>
        // public Client TrafficRelayTarget { get; set; }
        public uint? Cookie { get; set; }
        public NatClientIndex? ClientIndex { get; set; }
        public ClientInfo() : base()
        {
        }
    }
}