using System.Net;
using UniSpyServer.Servers.NatNegotiation.Handler;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Logging;

namespace UniSpyServer.Servers.NatNegotiation.Entity.Structure
{
    public class Client : ClientBase
    {
        public new ClientInfo Info { get => (ClientInfo)base.Info; private set => base.Info = value; }
        public new IUdpConnection Connection => (IUdpConnection)base.Connection;
        public Client(IConnection connection) : base(connection)
        {
            Info = new ClientInfo();
            _isLogRawMessage = true;
        }

        protected override ISwitcher CreateSwitcher(object buffer) => new CmdSwitcher(this, buffer);
    }
}