namespace QueryReport.Entity.Enumerator
{
    public enum QRErrorCode : uint
    {
        /// <summary>
        /// Error in request that send by client
        /// </summary>
        RequestError,
        /// <summary>
        /// Error in database
        /// </summary>
        DatabaseError,
        /// <summary>
        /// Error in networks
        /// </summary>
        NetworkError,
        /// <summary>
        /// No error in process data
        /// </summary>
        NoError
    }
}
