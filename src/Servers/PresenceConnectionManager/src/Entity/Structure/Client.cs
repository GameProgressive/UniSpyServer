using UniSpyServer.Servers.PresenceConnectionManager.Structure.Data;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure
{
    public class Client : ClientBase
    {
        public new ITcpSession Session => (ITcpSession)base.Session;
        public new ClientInfo Info { get => (ClientInfo)base.Info; private set => base.Info = value; }
        public Client(ISession session) : base(session)
        {
            Info = new ClientInfo(Session.RemoteIPEndPoint);
        }
    }
}