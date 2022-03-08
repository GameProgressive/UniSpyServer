using System;
using System.Net;

namespace UniSpyServer.UniSpyLib.Abstraction.Interface
{
    public interface IServer
    {
        Guid ServerID { get; }
        string ServerName { get; }
        IPEndPoint Endpoint { get; }
        bool Start();
    }
}
