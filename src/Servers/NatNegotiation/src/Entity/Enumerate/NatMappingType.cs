namespace UniSpyServer.NatNegotiation.Entity.Enumerate
{
    public enum NatMappingScheme : byte
    {
        Unrecognized,
        PrivateAsPublic,
        ConsistentPort,
        Incremental,
        Mixed,
        NumMappingSchemes
    }
}