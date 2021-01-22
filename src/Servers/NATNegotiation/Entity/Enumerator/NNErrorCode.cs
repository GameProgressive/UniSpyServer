namespace NATNegotiation.Entity.Enumerate
{
    public enum NNErrorCode
    {
        NoError,
        Parse,
        RequestError,
        MagicDataError,
        VersionError,
        PacketTypeError,
        CookieError,
        InitPacketError,
        ConnectPacketError,
        ReportPacketError
    }
}
