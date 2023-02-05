namespace UniSpyServer.Servers.NatNegotiation.Entity.Enumerate
{
    public enum NatStrategyType
    {
        UsePublicIP,
        /// <summary>
        /// currently we disable it
        /// </summary>
        UsePrivateIP,
        UseGameTrafficRelay
    }
}