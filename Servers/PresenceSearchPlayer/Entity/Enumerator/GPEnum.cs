

namespace PresenceSearchPlayer.Enumerator
{
    public enum GPEnum : uint
    {
        // Callbacks
        ////////////
        Error = 0,
        RecvBuddyRequest,
        RecvBuddyStatus,
        RecvBuddyMessage,
        RecvBuddyUTM,
        RecvGameInvite,
        TransferCallback,
        RecvBuddyAuth,
        RecvBuddyRevoke,

        // Global States.
        /////////////////
        InfoCaching = 0x0100,
        Simulation,
        InfoChachingBuddyAndBlockOnly,

        // Blocking
        ///////////
        Blocking = 1,
        NonBlocking = 0,

        // Firewall
        ///////////
        Firewall = 1,
        NoFirewall = 0,

        // Check Cache
        //////////////
        CheckCache = 1,
        DontCheckCache = 0,

        // Is Valid Email.
        // PANTS|02.15.00
        //////////////////
        EmailValid = 1,
        EmailInvalid = 0,

        // Fatal Error.
        ///////////////
        Fatal = 1,
        NonFatal = 0,

        // Sex
        //////
        Male = 0x0500,
        Fefamle,
        PAT,

        // Profile Search.
        //////////////////
        More = 0x0600,
        Done,

        // Set Info
        ///////////
        Nick = 0x0700,
        Uniquenick,
        Email,
        Password,
        FirstName,
        LastName,
        ICQUIN,
        HomePage,
        ZIPCode,
        CountryCode,
        Birthday,
        Sex,
        CPUBrand,
        CPUSpeed,
        Memory,
        VideoCard1String,
        VideoCard1RAM,
        VideoCard2String,
        VideoCard2RAM,
        ConnectionID,
        ConnectionSpeed,
        HasNetwork,
        OSString,
        AIMName,  // PANTS|03.20.01
        PIC,
        OccupationID,
        IndustryID,
        InComeID,
        MarriedID,
        ChildCount,
        Interest1,

        // New Profile.
        ///////////////
        Replace = 1,
        DontReplace = 0,

        // Is Connected.
        ////////////////
        Connected = 1,
        NotConnected = 0,

        // Public mask.
        ///////////////
        MaskNone = 0x00000000,
        MaskHomepage = 0x00000001,
        MaskZIPCode = 0x00000002,
        MaskContryCode = 0x00000004,
        MaskBirthday = 0x00000008,
        MaskSex = 0x00000010,
        MaskEmail = 0x00000020,
        MaskAll = 0xFFFFFFFF,

        // Status
        /////////
        Offline = 0,
        Online = 1,
        Playing = 2,
        Staging = 3,
        Chatting = 4,
        Away = 5,

        // Session flags
        /////////////////
        SessIsClosed = 0x00000001,
        SessIsOpen = 0x00000002,
        SessHasPassword = 0x00000004,
        SessIsBehindNAT = 0x00000008,
        SessIsRanked = 0x000000010,


        // CPU Brand ID
        ///////////////
        Intel = 1,
        AMD,
        CYRIX,
        Motorola,
        Alpha,

        // Connection ID.
        /////////////////
        Modem = 1,
        ISDN,
        CableModem,
        DSL,
        Satellite,
        Ethernet,
        Wireless,

        // Transfer callback type.
        // *** the transfer is ended when these types are received
        //////////////////////////
        TransferSendRequest = 0x800,  // arg->num == numFiles
        TransferAccepted,
        TransferRejected,        // ***
        TransferNotAccepting,   // ***
        TransferNoConnection,   // ***
        TransferDone,            // ***
        TransferCancelled,       // ***
        TransferLostConnection, // ***
        TransferError,           // ***
        TransferThrottle,  // arg->num == Bps
        FileBegin,
        FileProgress,  // arg->num == numBytes
        FileEnd,
        FileDirectory,
        FileSkip,
        FileFaild,  // arg->num == error

        //FILE_FAILED error
        ///////////////////////
        FileReadError = 0x900,
        FileWriteError,
        FileDataError,

        // Transfer Side.
        /////////////////
        TransferSender = 0xA00,
        TansferReciever,

        // UTM send options.
        ////////////////////
        DontRout = 0xB00, // only send direct

        // Quiet mode flags.
        ////////////////////
        SlienceNone = 0x00000000,
        SlienceMessage = 0x00000001,
        SlienceUTMS = 0x00000002,
        SlienceList = 0x00000004, // includes requests, auths, and revokes
        SlienceAll = 0xFFFFFFFF,

        NewStatusInfoSupported = 0xC00,
        NewStatusInfoNotSupported = 0xC01
    }
    public enum GPSPResult : uint
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
