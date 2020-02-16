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
        /// <summary>
        /// Not determined
        /// </summary>
        GP,
        /// <summary>
        /// solicited ERT reply
        /// </summary>
        NN1,
        /// <summary>
        /// unsolicited IP ERT reply
        /// </summary>
        NN2,
        /// <summary>
        /// unsolicited IP&Port ERT reply
        /// </summary>
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
