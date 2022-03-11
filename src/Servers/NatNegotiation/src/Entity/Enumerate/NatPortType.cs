namespace UniSpyServer.Servers.NatNegotiation.Entity.Enumerate
{
    public enum NatServerType : byte
    {
        /// <summary>
        /// Use game port for nat neg
        /// </summary>
        Map1A,
        /// <summary>
        /// IP and Port both restricted
        /// </summary>
        Map2,
        /// <summary>
        /// IP not restricted
        /// </summary>
        Map3,
        /// <summary>
        /// Port not restriced
        /// </summary>
        Map1B
    }
}