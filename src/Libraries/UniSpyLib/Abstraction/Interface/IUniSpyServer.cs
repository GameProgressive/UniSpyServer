using System;
using System.Net;

namespace UniSpyLib.Abstraction.Interface
{
    public interface IUniSpyServer
    {
        public Guid ServerID { get; }
        public IPEndPoint Endpoint { get; }
        public bool Start();
    }
}
