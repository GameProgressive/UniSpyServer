using System;
using System.Collections.Generic;
using System.Text;

namespace RetroSpyServer.Servers.GPSP.Enumerators
{
    public enum GPSPEnum:uint
    {
        // Callbacks
        ////////////
        GP_ERROR = 0,
        GP_RECV_BUDDY_REQUEST,
        GP_RECV_BUDDY_STATUS,
        GP_RECV_BUDDY_MESSAGE,
        GP_RECV_BUDDY_UTM,
        GP_RECV_GAME_INVITE,
        GP_TRANSFER_CALLBACK,
        GP_RECV_BUDDY_AUTH,
        GP_RECV_BUDDY_REVOKE,

        // Global States.
        /////////////////
        GP_INFO_CACHING = 0x0100,
        GP_SIMULATION,
        GP_INFO_CACHING_BUDDY_AND_BLOCK_ONLY,

        // Blocking
        ///////////
        GP_BLOCKING = 1,
        GP_NON_BLOCKING = 0,

        // Firewall
        ///////////
        GP_FIREWALL = 1,
        GP_NO_FIREWALL = 0,

        // Check Cache
        //////////////
        GP_CHECK_CACHE = 1,
        GP_DONT_CHECK_CACHE = 0,

        // Is Valid Email.
        // PANTS|02.15.00
        //////////////////
        GP_VALID = 1,
        GP_INVALID = 0,

        // Fatal Error.
        ///////////////
        GP_FATAL = 1,
        GP_NON_FATAL = 0,

        // Sex
        //////
        GP_MALE = 0x0500,
        GP_FEMALE,
        GP_PAT,

        // Profile Search.
        //////////////////
        GP_MORE = 0x0600,
        GP_DONE,

        // Set Info
        ///////////
        GP_NICK = 0x0700,
        GP_UNIQUENICK,
        GP_EMAIL,
        GP_PASSWORD,
        GP_FIRSTNAME,
        GP_LASTNAME,
        GP_ICQUIN,
        GP_HOMEPAGE,
        GP_ZIPCODE,
        GP_COUNTRYCODE,
        GP_BIRTHDAY,
        GP_SEX,
        GP_CPUBRANDID,
        GP_CPUSPEED,
        GP_MEMORY,
        GP_VIDEOCARD1STRING,
        GP_VIDEOCARD1RAM,
        GP_VIDEOCARD2STRING,
        GP_VIDEOCARD2RAM,
        GP_CONNECTIONID,
        GP_CONNECTIONSPEED,
        GP_HASNETWORK,
        GP_OSSTRING,
        GP_AIMNAME,  // PANTS|03.20.01
        GP_PIC,
        GP_OCCUPATIONID,
        GP_INDUSTRYID,
        GP_INCOMEID,
        GP_MARRIEDID,
        GP_CHILDCOUNT,
        GP_INTERESTS1,

        // New Profile.
        ///////////////
        GP_REPLACE = 1,
        GP_DONT_REPLACE = 0,

        // Is Connected.
        ////////////////
        GP_CONNECTED = 1,
        GP_NOT_CONNECTED = 0,

        // Public mask.
        ///////////////
        GP_MASK_NONE = 0x00000000,
        GP_MASK_HOMEPAGE = 0x00000001,
        GP_MASK_ZIPCODE = 0x00000002,
        GP_MASK_COUNTRYCODE = 0x00000004,
        GP_MASK_BIRTHDAY = 0x00000008,
        GP_MASK_SEX = 0x00000010,
        GP_MASK_EMAIL = 0x00000020,
        GP_MASK_ALL = 0xFFFFFFFF,

        // Status
        /////////
        GP_OFFLINE = 0,
        GP_ONLINE = 1,
        GP_PLAYING = 2,
        GP_STAGING = 3,
        GP_CHATTING = 4,
        GP_AWAY = 5,

        // Session flags
        /////////////////
        GP_SESS_IS_CLOSED = 0x00000001,
        GP_SESS_IS_OPEN = 0x00000002,
        GP_SESS_HAS_PASSWORD = 0x00000004,
        GP_SESS_IS_BEHIND_NAT = 0x00000008,
        GP_SESS_IS_RANKED = 0x000000010,


        // CPU Brand ID
        ///////////////
        GP_INTEL = 1,
        GP_AMD,
        GP_CYRIX,
        GP_MOTOROLA,
        GP_ALPHA,

        // Connection ID.
        /////////////////
        GP_MODEM = 1,
        GP_ISDN,
        GP_CABLEMODEM,
        GP_DSL,
        GP_SATELLITE,
        GP_ETHERNET,
        GP_WIRELESS,

        // Transfer callback type.
        // *** the transfer is ended when these types are received
        //////////////////////////
        GP_TRANSFER_SEND_REQUEST = 0x800,  // arg->num == numFiles
        GP_TRANSFER_ACCEPTED,
        GP_TRANSFER_REJECTED,        // ***
        GP_TRANSFER_NOT_ACCEPTING,   // ***
        GP_TRANSFER_NO_CONNECTION,   // ***
        GP_TRANSFER_DONE,            // ***
        GP_TRANSFER_CANCELLED,       // ***
        GP_TRANSFER_LOST_CONNECTION, // ***
        GP_TRANSFER_ERROR,           // ***
        GP_TRANSFER_THROTTLE,  // arg->num == Bps
        GP_FILE_BEGIN,
        GP_FILE_PROGRESS,  // arg->num == numBytes
        GP_FILE_END,
        GP_FILE_DIRECTORY,
        GP_FILE_SKIP,
        GP_FILE_FAILED,  // arg->num == error

        // GP_FILE_FAILED error
        ///////////////////////
        GP_FILE_READ_ERROR = 0x900,
        GP_FILE_WRITE_ERROR,
        GP_FILE_DATA_ERROR,

        // Transfer Side.
        /////////////////
        GP_TRANSFER_SENDER = 0xA00,
        GP_TRANSFER_RECEIVER,

        // UTM send options.
        ////////////////////
        GP_DONT_ROUTE = 0xB00, // only send direct

        // Quiet mode flags.
        ////////////////////
        GP_SILENCE_NONE = 0x00000000,
        GP_SILENCE_MESSAGES = 0x00000001,
        GP_SILENCE_UTMS = 0x00000002,
        GP_SILENCE_LIST = 0x00000004, // includes requests, auths, and revokes
        GP_SILENCE_ALL = 0xFFFFFFFF,

        NewStatusInfoSupported = 0xC00,
        NewStatusInfoNotSupported = 0xC01
    }
    public enum GPSPResult:int
    {
        NoError,
        MemoryError,
        ParameterError,
        NetworkError,
        ServerError,
        MISCError,
        Count
    }

    public enum GPSPNewUser:uint
    {
        NewUser = 0x0200,
        NewUserBadNick,
        NewUserBadPassword,
        NewUserUniquenickInvalid,
        NewUserUniqueNickINUSE,
    }

    public enum GPErrorCode:uint
    {
        // General.
        ///////////
        General = 0x0000,
        Parse,
        NotLoggedIn,
        BadSessionKey,
        DatabaseError,
        Network,
        ForcedDisconnect,
        ConnectionClose,
        UdpLayer,

        // Login.
        /////////
        Login = 0x0100,
        LoginTimeOut,
        LoginBadNick,
        LoginBadEmail,
        LoginBadPassword,
        LoginBadProfile,
        LoginProfileDeleted,
        LoginConnectionFailed,
        LoginServerAuthFaild,
        LoginBadUniquenick,
        LoginBadPreAuth,

        // Newuser.
        ///////////
        NewUser = 0x0200,
        NewUserBadNick,
        NewUserBadPasswords,
        NewUserUniquenickInvalid,
        NewUserUniquenickINUSE,        

        // Updateui.
        ////////////
        UpdateUI = 0x0300,
        UpdateUIBadEmail,

        // Newprofile.
        //////////////
        GP_NEWPROFILE = 0x0400,
        GP_NEWPROFILE_BAD_NICK,
        GP_NEWPROFILE_BAD_OLD_NICK,

        // Updatepro.
        /////////////
        GP_UPDATEPRO = 0x0500,
        GP_UPDATEPROz_BAD_NICK,

        // Addbuddy.
        ////////////
        GP_ADDBUDDY = 0x0600,
        GP_ADDBUDDY_BAD_FROM,
        GP_ADDBUDDY_BAD_NEW,
        GP_ADDBUDDY_ALREADY_BUDDY,

        // Authadd.
        ///////////
        GP_AUTHADD = 0x0700,
        GP_AUTHADD_BAD_FROM,
        GP_AUTHADD_BAD_SIG,

        // Status.
        //////////
        GP_STATUS = 0x0800,

        // Bm.
        //////
        GP_BM = 0x0900,
        GP_BM_NOT_BUDDY,
        GP_BM_EXT_INFO_NOT_SUPPORTED,
        GP_BM_BUDDY_OFFLINE,

        // Getprofile.
        //////////////
        GP_GETPROFILE = 0x0A00,
        GP_GETPROFILE_BAD_PROFILE,

        // Delbuddy.
        ////////////
        GP_DELBUDDY = 0x0B00,
        GP_DELBUDDY_NOT_BUDDY,

        // Delprofile.
        /////////////
        GP_DELPROFILE = 0x0C00,
        GP_DELPROFILE_LAST_PROFILE,

        // Search.
        //////////
        GP_SEARCH = 0x0D00,
        GP_SEARCH_CONNECTION_FAILED,
        GP_SEARCH_TIMED_OUT,

        // Check.
        /////////
        GP_CHECK = 0x0E00,
        GP_CHECK_BAD_EMAIL,
        GP_CHECK_BAD_NICK,
        GP_CHECK_BAD_PASSWORD,

        // Revoke.
        //////////
        GP_REVOKE = 0x0F00,
        GP_REVOKE_NOT_BUDDY,

        // Registeruniquenick.
        //////////////////////
        GP_REGISTERUNIQUENICK = 0x1000,
        GP_REGISTERUNIQUENICK_TAKEN,
        GP_REGISTERUNIQUENICK_RESERVED,
        GP_REGISTERUNIQUENICK_BAD_NAMESPACE,

        // Register cdkey.
        //////////////////
        GP_REGISTERCDKEY = 0x1100,
        GP_REGISTERCDKEY_BAD_KEY,
        GP_REGISTERCDKEY_ALREADY_SET,
        GP_REGISTERCDKEY_ALREADY_TAKEN,

        // AddBlock.
        ////////////
        GP_ADDBLOCK = 0x1200,
        GP_ADDBLOCK_ALREADY_BLOCKED,

        // RemoveBlock.
        ///////////////
        RemoveBlock = 0x1300,
        RemoveBlockNotBlocked,


            NoError =0xffff
    }
}
