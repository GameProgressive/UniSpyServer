namespace UniSpyServer.Servers.NatNegotiation.Entity.Enumerate
{
    public enum NatNegResult
    {
        /// <summary>
        /// Two client are successfully connected
        /// client -> server
        /// <summary>
        Success,
        /// <summary>
        /// The partner is not registered to nat server
        /// server -> client
        /// <summary>
        DeadBeatPartner,
        /// <summary>
        /// client -> server
        /// <summary>
        InitTimeOut,
        /// <summary>
        /// The ping packet is timeout
        /// client -> server
        /// <summary>
        PingTimeOut,
        /// <summary>
        /// client -> server
        /// Unknown error happen in natneg
        /// <summary>
        UnknownError,
        NoResult
    }
}