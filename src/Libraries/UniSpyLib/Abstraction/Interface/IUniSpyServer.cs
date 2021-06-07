using System;
using System.Net;
using UniSpyLib.Abstraction.BaseClass;

namespace UniSpyLib.Abstraction.Interface
{
    public interface IUniSpyServer
    {
        Guid ServerID { get; }
        IPEndPoint Endpoint { get; }
        UniSpySessionManagerBase SessionManager { get; }
        bool Start();
    }
}
