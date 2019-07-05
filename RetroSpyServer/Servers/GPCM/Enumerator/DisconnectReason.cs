namespace RetroSpyServer.Servers.GPCM.Enumerator
{
    public enum DisconnectReason : int
    {
        /// <summary>
        /// Client sends the "logout" command
        /// </summary>
        NormalLogout,

        /// <summary>
        /// Keep Alive Packet failed to send (may not work with new async socket code)
        /// </summary>
        KeepAliveFailed,

        /// <summary>
        /// The client failed to complete the login 15 seconds after creating the connection
        /// </summary>
        LoginTimedOut,

        /// <summary>
        /// The username sent by the client does not exist
        /// </summary>
        InvalidUsername,

        /// <summary>
        /// The provided password for the player nick is incorrect
        /// </summary>
        InvalidPassword,

        /// <summary>
        /// Invalid login query sent by client
        /// </summary>
        InvalidLoginQuery,

        /// <summary>
        /// Create player failed, username exists already
        /// </summary>
        CreateFailedUsernameExists,

        /// <summary>
        /// Failed to create new player account due to a database exception
        /// </summary>
        CreateFailedDatabaseError,

        /// <summary>
        /// A general login failure (check error log)
        /// </summary>
        GeneralError,

        /// <summary>
        /// The stream disconnected unexpectedly. This can happen if the user clicks the
        /// "Quit" button on the top of the main menu instead of the "Logout" button.
        /// </summary>
        Disconnected,

        /// <summary>
        /// The player was forcefully logged out by console command
        /// </summary>
        ForcedLogout,

        /// <summary>
        /// A new login detected with the old player session still logged in
        /// </summary>
        NewLoginDetected,

        /// <summary>
        /// Forced server shutdown
        /// </summary>
        ForcedServerShutdown,

        /// <summary>
        /// The client challenge was already sent by the server for this connection
        /// </summary>
        ClientChallengeAlreadySent,

        /// <summary>
        /// The player is banned and cannot login
        /// </summary>
        PlayerIsBanned,

        /// <summary>
        /// The player information is not valid
        /// </summary>
        InvalidPlayer,

        /// <summary>
        /// The player account is not activated
        /// </summary>
        PlayerIsNotActivated
    }
}
