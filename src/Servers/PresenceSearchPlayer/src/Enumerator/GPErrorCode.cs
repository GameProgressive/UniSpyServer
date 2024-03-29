namespace UniSpy.Server.PresenceSearchPlayer.Enumerate
{
    public enum GPErrorCode : int
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
        LoginBadLoginTicket,
        LoginTicketExpired,
        // Newuser.
        ///////////
        NewUser = 0x0200,
        NewUserBadNick,
        NewUserBadPasswords,
        NewUserUniquenickInvalid,
        NewUserUniquenickInUse,

        // Updateui.
        ////////////
        UpdateUI = 0x0300,
        UpdateUIBadEmail,

        // Newprofile.
        //////////////
        NewProfile = 0x0400,
        NewProfileBadNick,
        NewProfileBadOldNick,

        // Updatepro.
        /////////////
        UpdatePro = 0x0500,
        UpdateProBadNick,

        // Addbuddy.
        ////////////
        AddBuddy = 0x0600,
        AddBuddyBadForm,
        AddBuddyBadNew,
        AddBuddyAlreadyBuddy,

        // Authadd.
        ///////////
        AuthAdd = 0x0700,
        AuthAddBadForm,
        AuthAddBadSig,

        // Status.
        //////////
        Status = 0x0800,

        // Bm.
        //////
        Bm = 0x0900,
        BmNotBuddy,
        BmExtInfoNotSupported,
        BmBuddyOffline,

        // Getprofile.
        //////////////
        GetProfile = 0x0A00,
        GetProfileBadProfile,

        // Delbuddy.
        ////////////
        DelBuddy = 0x0B00,
        DelBuddyNotBuddy,

        // Delprofile.
        /////////////
        DelProfile = 0x0C00,
        DelProfileLastProfile,

        // Search.
        //////////
        Search = 0x0D00,
        SearchConnectionFailed,
        SearchTimeOut,

        // Check.
        /////////
        Check = 0x0E00,
        CheckBadMail,
        CheckBadNick,
        CheckBadPassword,

        // Revoke.
        //////////
        Revoke = 0x0F00,
        RevokeNotBuddy,

        // Registeruniquenick.
        //////////////////////
        RegisterUniquenick = 0x1000,
        RegisterUniquenickTaken,
        RegisterUniquenickReserved,
        RegisterUniquenickBadNamespace,

        // Register UniSpy.Server.CDkey.
        //////////////////
        RegisterCDKey = 0x1100,
        RegisterCDKeyBadKey,
        RegisterCDKeyAlreadySet,
        RegisterCDKeyAlreadyTaken,

        // AddBlock.
        ////////////
        AddBlock = 0x1200,
        AddBlockAlreadyBlocked,

        // RemoveBlock.
        ///////////////
        RemoveBlock = 0x1300,
        RemoveBlockNotBlocked,


        // RetroSpy self defined NoError code
        ///////////////
        NoError = 0xffff,
    }
}
