using System;
using System.Collections.Generic;
using System.Text;

namespace RetroSpyServer.Servers.GPSP.Enumerators
{
    public enum GPSPEnum
    {
        NewStatusInfoSupported = 0xC00,
        NewStatusInfoNotSupported = 0xC01
    }
    public enum GPSPResult
    {
        NoError,
        MemoryError,
        ParameterError,
        NetworkError,
        ServerError,
        MISCError,
        Count
    }
}
