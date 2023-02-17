
namespace UniSpy.Server.NatNegotiation.Enumerate
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
        Ping = 7,
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
    public enum NatClientIndex : byte
    {
        GameClient = 0,
        GameServer = 1
    }
}