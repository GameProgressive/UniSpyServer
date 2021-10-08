namespace NatNegotiation.Entity.Enumerate
{
    public enum ConnectPacketStatus : byte
    {
        NoError,
        DeadHeartBeat,
        InitPacketTimeOut
    }
}
