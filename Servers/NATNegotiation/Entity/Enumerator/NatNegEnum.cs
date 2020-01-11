namespace NATNegotiation.Entity.Enumerator
{

    public enum NatPacketType : byte
    {
        Init = 0x00,
        InitAck = 0x01,
        ErtTest = 0x02,
        ErtAck = 0x03,
        StateUpdate = 0x04,
        Connect = 0x05,
        ConnectAck = 0x06,
        ConnectPing = 0x07,
        BackupTest = 0x08,
        BackupAck = 0x09,
        AddressCheck = 0x0A,
        AddressReply = 0x0B,
        NatifyRequest = 0x0C,
        Report = 0x0D,
        ReportAck = 0x0E,
        PreInit = 0x0F,
        PreInitAck = 0x10,
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
