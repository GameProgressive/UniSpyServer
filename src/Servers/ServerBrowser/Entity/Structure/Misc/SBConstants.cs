using UniSpyLib.Extensions;

namespace ServerBrowser.Entity.Structure.Misc
{
    internal sealed class SBConstants
    {
        internal const string LocalIP0 = "localip0";
        internal const string LocalIP1 = "localip1";
        internal const string PrivatePort = "privateport";
        internal const string HostPort = "hostport";
        internal const string HostPortNumber = "6500";
        /// <summary>
        /// we are not gamespy,
        /// for simplicity we use hard coded challenge
        /// to reduce computation cost of our program
        /// </summary>
        internal const string ServerChallenge = "0000000000";
        internal const ushort QueryReportDefaultPort = 6500;
        internal static byte[] HtonQueryReportDefaultPort
        => ByteTools.GetBytes(SBConstants.QueryReportDefaultPort, true);

    }
}