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

    public enum GPSPNewUser
    {
        NewUser = 0x0200,
        NewUserBadNick,
        NewUserBadPassword,
        NewUserUniquenickInvalid,
        NewUserUniqueNickINUSE,
    }

    public enum GPErrorCode
    {
        General = 0x0000,
        Parse,
        NotLoggedIn,
        BadSessionKey,
        DatabaseError,
        Network,
        ForcedDisconnect,
        ConnectionClose,
        UdpLayer,
    }
}
