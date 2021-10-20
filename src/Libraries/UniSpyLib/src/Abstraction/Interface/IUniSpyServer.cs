using System;
using System.Net;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass.Network;

namespace UniSpyServer.UniSpyLib.Abstraction.Interface
{
    public interface IUniSpyServer
    {
        Guid ServerID { get; }
        IPEndPoint Endpoint { get; }
        UniSpySessionManager SessionManager { get; }
        bool Start();
    }
}
