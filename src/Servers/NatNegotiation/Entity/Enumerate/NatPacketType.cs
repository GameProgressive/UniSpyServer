
namespace NatNegotiation.Entity.Enumerate
{
    public enum NatPacketType : byte
    {
        /// <summary>
        /// Initialize nat negotiation with cookie
        /// </summary>
        Init,
        InitAck,
        /// <summary>
        /// External reachability test
        /// </summary>
        ErtTest,
        ErtAck,
        StateUpdate,
        /// <summary>
        /// Notify participant to negotiate
        /// </summary>
        Connect,
        ConnectAck,
        /// <summary>
        /// Connect other participant with ping packet
        /// </summary>
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
}