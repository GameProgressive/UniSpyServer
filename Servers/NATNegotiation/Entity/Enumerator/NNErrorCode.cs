using System;
using System.Collections.Generic;
using System.Text;

namespace NATNegotiation.Entity.Enumerator
{
    public enum NNErrorCode
    {
        RequestError,
        MagicDataError,
        VersionError,
        PacketTypeError,
        CookieError,
        InitPacketError,
        ConnectPacketError,
        ReportPacketError,
        NoError
    }
}
