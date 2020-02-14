using System;
using System.Collections.Generic;
using System.Text;

namespace NATNegotiation.Entity.Structure
{
    public enum NNErrorCode
    {
        RequestIncorrect,
        MagicDataIncorrect,
        VersionIncorrect,
        PacketTypeIncorrect,
        CookieIncorrect,
        InitPacketIncorrect,
        ConnectPacketIncorrect,
        ReportPacketIncorrect,
        NoError
    }
}
