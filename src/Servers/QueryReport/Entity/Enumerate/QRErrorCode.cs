namespace QueryReport.Entity.Enumerate
{
    public enum ErrorCode : uint
    {
        General,
        /// <summary>
        /// Error in request that send by client
        /// </summary>
        Parse,
        /// <summary>
        /// Error in database
        /// </summary>
        Database,
        /// <summary>
        /// Error in networks
        /// </summary>
        Network,
        /// <summary>
        /// No error in process data
        /// </summary>
        NoError
    }
}
