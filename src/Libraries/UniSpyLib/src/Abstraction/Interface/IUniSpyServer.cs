using System;
using System.Net;
using UniSpyLib.Abstraction.BaseClass.Network;

namespace UniSpyLib.Abstraction.Interface
{
    public interface IUniSpyServer
    {
        Guid ServerID { get; }
        IPEndPoint Endpoint { get; }
        UniSpySessionManager SessionManager { get; }
        bool Start();
    }
}
