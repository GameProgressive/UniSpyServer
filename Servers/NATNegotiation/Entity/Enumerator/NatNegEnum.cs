namespace NatNegotiation.Entity.Enumerator
{
    public enum ConnectPacketFinishStatus : byte
    {
        NoError,
        DeadHeartBeat,
        InitPacketTimeOut
    }

    public enum NatPortType : byte
    {
        /// <summary>
        /// Use game port for nat neg
        /// </summary>
        GP,
        /// <summary>
        /// IP and Port both restricted
        /// </summary>
        NN1,
        /// <summary>
        /// IP not restricted
        /// </summary>
        NN2,
        /// <summary>
        /// Port not restriced
        /// </summary>
        NN3
    }

    public enum NatPacketType : byte
    {
        Init,
        InitAck,
        ErtTest,
        ErtAck,
        StateUpdate,
        Connect,
        ConnectAck,
        ConnectPing,
        BackupTest,
        BackupAck,
        AddressCheck,
        AddressReply,
        NatifyRequest,
        Report,
        ReportAck,
        PreInit,
        PreInitAck,
    }

    internal enum NatifyPacket : byte
    {
        PacketMap1a,
        PacketMap2,
        PacketMap3,
        PacketMap1b,
        NumPackets
    }

    public enum NatMappingScheme : byte
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

    internal enum NatPromiscuity
    {
        Promiscuous,
        NotPromiscuous,
        PortPromiscuous,
        IpPromiscuous,
        PromiscuityNotApplicable,
        NumPromiscuityTypes
    }
}
