namespace UniSpyServer.Servers.NatNegotiation
{

    public enum NatPunchStrategy
    {
        /// <summary>
        /// If 2 clients are guessed in same LAN and natneg fail we use public ip to negotiate
        /// </summary>
        UsingPublicIPEndPoint,
        /// <summary>
        /// If 2 clients are probabaly in same LAN, we use private ip to negotiate
        /// </summary>
        UsingPrivateIPEndpoint,
        /// <summary>
        /// If 2 clients are failed with public ip negotation, we use game relay service to redirect the game traffic
        /// </summary>
        UsingGameRelay
    }
}