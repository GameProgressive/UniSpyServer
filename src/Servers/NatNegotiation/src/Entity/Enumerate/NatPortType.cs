namespace UniSpyServer.Servers.NatNegotiation.Entity.Enumerate
{
    public enum NatPortType : byte
    {
        /// <summary>
        /// Use game port for nat neg
        /// </summary>
        GamePort,
        /// <summary>
        /// IP and Port both restricted
        /// </summary>
        Natneg1,
        /// <summary>
        /// IP not restricted
        /// </summary>
        Natneg2,
        /// <summary>
        /// Port not restriced
        /// </summary>
        Natneg3
    }
}