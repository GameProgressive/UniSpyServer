
namespace UniSpyServer.Servers.NatNegotiation.Entity.Enumerate
{
    public enum RequestType : byte
    {
        /// <summary>
        /// Initialize nat negotiation with cookie
        /// </summary>
        Init = 0,
        ErtAck = 3,
        Connect = 5,
        ConnectAck = 6,
        AddressCheck = 10,
        NatifyRequest = 12,
        Report = 13,
        PreInit = 15
    }
    public enum ResponseType : byte
    {
        InitAck = 1,
        ErtTest = 2,
        ErtAck = 3,
        Connect = 5,
        AddressReply = 11,
        ReportAck = 14,
        PreInitAck = 16
    }
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
        /// <summary>
        /// client received connect response
        /// </summary>
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