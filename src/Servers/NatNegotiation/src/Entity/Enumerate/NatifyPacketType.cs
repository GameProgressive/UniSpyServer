namespace UniSpyServer.Servers.NatNegotiation.Entity.Enumerate
{
    public enum NatifyPacketType : byte
    {
        PacketMap1a,
        PacketMap2,
        PacketMap3,
        PacketMap1b,
        NumPackets
    }

    public enum PreInitState : byte
    {
        WaitingForClient = 0,
        WaitingForMatchUp = 1,
        Ready = 2
    }
}