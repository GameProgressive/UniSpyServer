namespace UniSpyServer.Servers.NatNegotiation.Entity.Enumerate
{
    public enum ConnectPacketStatus : byte
    {
        NoError,
        DeadHeartBeat,
        InitPacketTimeOut
    }
}
