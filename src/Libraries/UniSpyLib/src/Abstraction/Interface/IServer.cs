using System;
using System.Net;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass.Network;

namespace UniSpyServer.UniSpyLib.Abstraction.Interface
{
    public interface IServer
    {
        Guid ServerID { get; }
        IPEndPoint Endpoint { get; }
        bool Start();
    }
}
