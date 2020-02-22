using System;
namespace QueryReport.Entity.Enumerator
{
    public enum QRPacketType : byte
    {
        Query = 0x00,
        Challenge = 0x01,
        Echo = 0x02,
        Heartbeat = 0x03,
        AddressError = 0x04,
        EchoResponse = 0x05,
        ClientMessage = 0x06,
        ClientMessageACK = 0x07,
        KeepAlive = 0x08,
        RequireIPVerify = 0x09
    }
}
