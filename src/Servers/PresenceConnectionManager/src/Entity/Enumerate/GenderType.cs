namespace UniSpy.Server.PresenceConnectionManager.Entity.Enumerate
{
    public enum GenderType : ushort
    {
        /// <summary>
        /// Gender is male
        /// </summary>
        Male,

        /// <summary>
        /// Gender is female
        /// </summary>
        Female,

        /// <summary>
        /// Unspecified or unknown gender, this is
        /// used to mask the gender when the information is queried
        /// </summary>
        Pat
    }
}
