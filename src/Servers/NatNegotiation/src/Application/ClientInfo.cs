using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Collections.Concurrent;
using UniSpyServer.Servers.NatNegotiation.Entity.Enumerate;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;

namespace UniSpyServer.Servers.NatNegotiation.Application
{
    public sealed class ClientInfo : ClientInfoBase
    {
        public uint? Cookie { get; set; }
        public NatClientIndex? ClientIndex { get; set; }
        public bool IsNeigotiating { get; set; }
        public Dictionary<NatPortType, bool> InitInfoStorageState { get; }
        public ClientInfo() : base()
        {
            InitInfoStorageState = new Dictionary<NatPortType, bool>();
        }
    }
}