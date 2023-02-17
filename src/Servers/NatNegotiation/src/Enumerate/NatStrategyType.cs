namespace UniSpy.Server.NatNegotiation.Enumerate
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