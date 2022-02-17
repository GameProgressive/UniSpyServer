using UniSpyServer.Servers.PresenceConnectionManager.Structure.Data;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure
{
    public class Client : ClientBase
    {
        public new ITcpConnection Connection => (ITcpConnection)base.Connection;
        public new ClientInfo Info { get => (ClientInfo)base.Info; private set => base.Info = value; }
        public Client(IConnection session) : base(session)
        {
            Info = new ClientInfo(Connection.RemoteIPEndPoint);
        }
    }
}