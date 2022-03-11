namespace UniSpyServer.Servers.NatNegotiation.Entity.Enumerate
{
    public enum NatPromiscuity : byte
    {
        Promiscuous,
        NotPromiscuous,
        PortPromiscuous,
        IPPromiscuous,
        PromiscuityNotApplicable
    }
    public enum NatType : byte
    {
        /// <summary>
        /// There are no NAT in this client
        /// </summary>
        NoNat,
        /// <summary>
        /// There maybe firewall in this client, some packet has been dropped
        /// </summary>
        FirewallOnly,
        /// <summary>
        ///内网主机建立一个UDP socket(LocalIP:LocalPort) 第一次使用这个socket给外部主机发送数据时NAT会给其分配一个公网(PublicIP:PublicPort),以后用这个socket向外面任何主机发送数据都将使用这对(PublicIP:PublicPort)。此外，任何外部主机只要知道这个(PublicIP:PublicPort)就可以发送数据给(PublicIP:PublicPort)，内网的主机就能收到这个数据包
        /// </summary>
        FullCone,
        /// <summary>
        /// 内网主机建立一个UDP socket(LocalIP:LocalPort) 第一次使用这个socket给外部主机发送数据时NAT会给其分配一个公网(PublicIP:PublicPort),以后用这个socket向外面任何主机发送数据都将使用这对(PublicIP:PublicPort)。此外，如果任何外部主机想要发送数据给这个内网主机，只要知道这个(PublicIP:PublicPort)并且内网主机之前用这个socket曾向这个外部主机IP发送过数据。只要满足这两个条件，这个外部主机就可以用自己的(IP,任何端口)发送数据给(PublicIP:PublicPort)，内网的主机就能收到这个数据包
        /// </summary>
        RestrictedCone,
        /// <summary>
        ///内网主机建立一个UDP socket(LocalIP:LocalPort) 第一次使用这个socket给外部主机发送数据时NAT会给其分配一个公网(PublicIP:PublicPort),以后用这个socket向外面任何主机发送数据都将使用这对(PublicIP:PublicPort)。此外，如果任何外部主机想要发送数据给这个内网主机，只要知道这个(PublicIP:PublicPort)并且内网主机之前用这个socket曾向这个外部主机(IP,Port)发送过数据。只要满足这两个条件，这个外部主机就可以用自己的(IP,Port)发送数据给(PublicIP:PublicPort)，内网的主机就能收到这个数据包
        /// </summary>
        PortRestrictedCone,
        /// <summary>
        /// 内网主机建立一个UDP socket(LocalIP,LocalPort),当用这个socket第一次发数据给外部主机1时,NAT为其映射一个(PublicIP-1,Port-1),以后内网主机发送给外部主机1的所有数据都是用这个(PublicIP-1,Port-1)，如果内网主机同时用这个socket给外部主机2发送数据，第一次发送时，NAT会为其分配一个(PublicIP-2,Port-2), 以后内网主机发送给外部主机2的所有数据都是用这个(PublicIP-2,Port-2).如果NAT有多于一个公网IP，则PublicIP-1和PublicIP-2可能不同，如果NAT只有一个公网IP,则Port-1和Port-2肯定不同，也就是说一定不能是PublicIP-1等于 PublicIP-2且Port-1等于Port-2。此外，如果任何外部主机想要发送数据给这个内网主机，那么它首先应该收到内网主机发给他的数据，然后才能往回发送，否则即使他知道内网主机的一个(PublicIP,Port)也不能发送数据给内网主机，这种NAT无法实现UDP-P2P通信。
        /// </summary>
        Symmetric,
        /// <summary>
        /// 未知的NAT类型
        /// </summary>
        Unknown
    }

    public enum NatPortMappingScheme : byte
    {
        // can not determine the mapping type
        Unrecognized,
        /// <summary>
        /// use the same public port for private port - each request
        /// <summary>
        PrivateAsPublic,
        /// <summary>
        /// use the same public port for private port - all requests
        /// </summary>
        ConsistentPort,
        /// <summary>
        /// use the incremental port
        /// <summary>
        Incremental,
        /// <summary>
        /// use mixed port mapping scheme
        /// <summary>
        Mixed,
    }
}