namespace NatNegotiation.Entity.Enumerate
{
    public enum NNErrorCode
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
