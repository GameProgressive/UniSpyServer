namespace NATNegotiation.Entity.Enumerate
{
    internal enum NNErrorCode
    {
        NoError,
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
