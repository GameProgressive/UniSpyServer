namespace NatNegotiation.Entity.Enumerator
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
