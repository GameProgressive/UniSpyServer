using System;
using System.Collections.Generic;
using System.Text;

namespace QueryReport.Structure
{
    public static class QR
    {
        public const byte QRMagic1 = 0xFE;
        public const byte QRMagic2 = 0XFD;
        public const int MaxDataSize = 1400;
        public const int RequestKeyLenth = 256;
        public const short PublicAddressLenth = 12;
        public const int QRPingTime = 120;
        public const int ChallengeLenth = 20;
    }
    public static class QRClient
    {
        public const byte Heartbeat = 0x03;//C -> S
        public const byte EchoResponse = 0x05; // 0x05, not 0x03 (order) | C -> S
        public const byte ClientMessageACK = 0x07; //C -> S
        public const byte KeepAlive = 0x08; //S -> C | C -> S
        public const byte Avaliable = 0x09; //C -> S
    }
    public static class QRGameServer
    {
        public const byte Query = 0x00; //S -> C
        public const byte Challenge = 0x01; //S -> C
        public const byte Echo = 0x02; //S -> C (purpose..?)
        public const byte AddressError = 0x04; //S -> C
        public const byte ClientMessage = 0x06; //S -> C
        public const byte RequireIPVerify = 0x09; //S -> C
        public const byte ClientRegistered = 0x0A; //S -> C
    }
}
