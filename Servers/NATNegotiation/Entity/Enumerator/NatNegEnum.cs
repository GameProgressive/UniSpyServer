namespace NatNegotiation.Entity.Enumerator
{

    public enum NatPacketType : byte
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
    public enum NatMappingScheme
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
