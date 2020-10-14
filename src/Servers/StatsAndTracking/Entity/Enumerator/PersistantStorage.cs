namespace StatsAndTracking.Entity.Enumerator
{
    public enum PersistStorageType : uint
    {
        /// <summary>
        /// Readable only by the authenticated client it belongs to, can only by set on the server
        /// </summary>
        PrivateRO,
        /// <summary>
        /// Readable only by the authenticated client it belongs to, set by the authenticated client it belongs to
        /// </summary>
        PrivateRW,
        /// <summary>
        /// Readable by any client, can only be set on the server
        /// </summary>
        PublicRO,
        /// <summary>
        /// Readable by any client, set by the authenicated client is belongs to
        /// </summary>
        PublicRW,
    }
}
