namespace PresenceConnectionManager.Entity.Enumerator
{
    /// <summary>
    /// The status of the player
    /// </summary>
    public enum PlayerStatus : uint
    {
        /// <summary>
        /// The player is offline
        /// </summary>
        Offline = 0,

        /// <summary>
        /// The player is online
        /// </summary>
        Online,

        /// <summary>
        /// The player is playing a game
        /// </summary>
        Playing,

        /// <summary>
        /// Unknown?
        /// </summary>
        Staging,

        /// <summary>
        /// The player is chatting?
        /// </summary>
        Chatting,

        /// <summary>
        /// The player is away from the computer
        /// </summary>
        Away,

        /// <summary>
        /// The player is banned
        /// </summary>
        Banned,
    };
}
