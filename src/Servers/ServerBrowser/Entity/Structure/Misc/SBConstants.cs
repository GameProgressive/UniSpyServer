namespace ServerBrowser.Entity.Structure.Misc
{
    internal class SBConstants
    {
        internal static string LocalIP0 = "localip0";
        internal static string LocalIP1 = "localip1";
        internal static string PrivatePort = "privateport";
        internal static string HostPort = "hostport";
        internal static string HostPortNumber = "6500";
        /// <summary>
        /// we are not gamespy,
        /// for simplicity we use hard coded challenge
        /// to reduce computation cost of our program
        /// </summary>
        internal static string ServerChallenge = "0000000000";
    }
}