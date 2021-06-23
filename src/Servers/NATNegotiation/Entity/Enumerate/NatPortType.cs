namespace NatNegotiation.Entity.Enumerate
{
    public enum NatPortType : byte
    {
        /// <summary>
        /// Use game port for nat neg
        /// </summary>
        GP,
        /// <summary>
        /// IP and Port both restricted
        /// </summary>
        NN1,
        /// <summary>
        /// IP not restricted
        /// </summary>
        NN2,
        /// <summary>
        /// Port not restriced
        /// </summary>
        NN3
    }
}