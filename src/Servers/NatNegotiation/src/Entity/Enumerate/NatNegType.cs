namespace UniSpyServer.NatNegotiation.Entity.Enumerate
{
    public enum NatNegType
    {
        NoNat,
        FirewallOnly,
        FullCone,
        RestrictedCone,
        PortRestrictedCone,
        Symmetric,
        Unknown,
        NumNatTypes
    }

    public enum NatPromiscuity
    {
        Promiscuous,
        NotPromiscuous,
        PortPromiscuous,
        IPPromiscuous,
        PromiscuityNotApplicable,
        NumPromiscuityTypes
    }


}
