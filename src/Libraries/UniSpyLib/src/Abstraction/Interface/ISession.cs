﻿using System.Net;

namespace UniSpyServer.UniSpyLib.Abstraction.Interface
{
    public interface ISession
    {
        EndPoint RemoteEndPoint { get; }
        IPEndPoint RemoteIPEndPoint { get; }
        bool Send(IResponse response);
    }
}