namespace NatNegotiation.Entity.Enumerator
{
    public enum ConnectPacketFinishStatus:uint
    {
        NoError,
        DeadHeartBeat,
        InitPacketTimeOut
    }
    public enum NatPortType
    {
        GP,
        NN1,
        NN2,
        NN3
    }
    public enum NatPacketType
    {
        Init ,
        InitAck,
        ErtTest,
        ErtAck,
        StateUpdate ,
        Connect ,
        ConnectAck ,
        ConnectPing ,
        BackupTest ,
        BackupAck ,
        AddressCheck,
        AddressReply,
        NatifyRequest ,
        Report ,
        ReportAck ,
        PreInit ,
        PreInitAck ,
    }
    enum NatifyPacket
    {
        PacketMap1a,
        PacketMap2,
        PacketMap3,
        PacketMap1b,
        NumPackets
    };
    public enum NatMappingScheme: byte
    {
        Unrecognized,
        PrivateAsPublic,
        ConsistentPort,
        Incremental,
        Mixed,
        NumMappingSchemes
    }
    public enum NatType
    {
        NoNat,
        FirewallOnly,
        FullCone,
        RestrictedCone,
        PortRestrictedCone,
        Symmetric,
        Unknown,
        NumNatTypes
    }
    enum NatPromiscuity
    {
        Promiscuous,
        NotPromiscuous,
        PortPromiscuous,
        IpPromiscuous,
        PromiscuityNotApplicable,
        NumPromiscuityTypes
    }
}
