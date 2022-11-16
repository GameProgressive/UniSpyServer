using System;
using System.Net;

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
        public static string CreateKey(IPAddress myPublicIPAddress, IPAddress myPrivateIPAddress, Guid otherClientServerId, IPAddress otherClientPrivateIP)
        {
            return $"{myPublicIPAddress} {myPrivateIPAddress} {otherClientServerId} {otherClientPrivateIP}";
        }
    }
}