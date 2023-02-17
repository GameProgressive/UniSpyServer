namespace UniSpy.Server.NatNegotiation.Enumerate
{
    public enum ConnectPacketStatus : byte
    {
        NoError,
        DeadHeartBeat,
        InitPacketTimeOut
    }
}
