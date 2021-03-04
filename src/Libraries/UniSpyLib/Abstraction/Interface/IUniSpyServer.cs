using System;
using System.Net;
using UniSpyLib.Abstraction.BaseClass;

namespace UniSpyLib.Abstraction.Interface
{
    public interface IUniSpyServer
    {
        public Guid ServerID { get; }
        public IPEndPoint Endpoint { get; }
        public UniSpySessionManagerBase SessionManager { get; }
        public bool Start();
    }
}
