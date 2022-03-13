namespace UniSpyServer.Servers.NatNegotiation.Entity.Enumerate
{
    public enum NatType : byte
    {
        NoNat,
        FirewallOnly,
        /// <summary>
        /// C发数据到210.21.12.140:8000，NAT会将数据包送到A（192.168.0.4:5000）。因为NAT上已经有了192.168.0.4:5000到210.21.12.140:8000的映射
        /// </summary>
        FullCone,
        /// <summary>
        /// C无法和A通信，因为A从来没有和C通信过，NAT将拒绝C试图与A连接的动作。但B可以通过210.21.12.140:8000与A的 192.168.0.4:5000通信，且这里B可以使用任何端口与A通信。如：210.15.27.166:2001 -> 210.21.12.140:8000，NAT会送到A的5000端口上
        /// </summary>
        AddressRestrictedCone,
        /// <summary>
        /// C无法与A通信，因为A从来没有和C通信过。而B也只能用它的210.15.27.166:2000与A的192.168.0.4:5000通信，因为A也从来没有和B的其他端口通信过。该类型NAT是端口受限的。
        /// </summary>
        PortRestrictedCone,
        /// <summary>
        /// 上面3种类型，统称为Cone NAT，有一个共同点：只要是从同一个内部地址和端口出来的包，NAT都将它转换成同一个外部地址和端口。但是Symmetric有点不同，具体表现在： 只要是从同一个内部地址和端口出来，且到同一个外部目标地址和端口，则NAT也都将它转换成同一个外部地址和端口。但如果从同一个内部地址和端口出来，是 到另一个外部目标地址和端口，则NAT将使用不同的映射，转换成不同的端口（外部地址只有一个，故不变）。而且和Port Restricted Cone一样，只有曾经收到过内部地址发来包的外部地址，才能通过NAT映射后的地址向该内部地址发包。
        /// </summary>
        Symmetric,
        Unknown
    }
    public enum NatPromiscuty : byte
    {
        Promiscuous,
        NotPromiscuous,
        PortPromiscuous,
        IpPromiscuous,
        PromiscuityNotApplicable
    }
    public enum NatPortMappingScheme : byte
    {
        Unrecognized,
        PrivateAsPublic,
        ConsistentPort,
        Incremental,
        Mixed,
    }
}