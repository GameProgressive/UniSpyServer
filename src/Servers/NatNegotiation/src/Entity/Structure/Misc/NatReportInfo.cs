using System;
using System.Net;
using UniSpyServer.Servers.NatNegotiation.Entity.Enumerate;

namespace UniSpyServer.Servers.NatNegotiation.Entity.Structure.Misc
{
    public class NatReportRecord
    {
        public Guid ServerId { get; init; }
        public IPAddress PublicIPAddress { get; init; }
        public IPAddress PrivateIPAddress { get; init; }
        public NatPunchStrategy CurrentPunchStrategy { get; set; }
    }
    public class NatReportInfo
    {
        public NatReportRecord MyRecord;
        public NatReportRecord OthersRecord;
        public static string CreateKey(IPAddress myPublicIPAddress, IPAddress myPrivateIPAddress, Guid otherClientServerId, IPAddress otherClientPrivateIP, NatClientIndex clientIndex)
        {
            return $"{myPublicIPAddress} {myPrivateIPAddress} {otherClientServerId} {otherClientPrivateIP} {clientIndex}";
        }
    }
}