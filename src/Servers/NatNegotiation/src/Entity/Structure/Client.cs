using UniSpyServer.UniSpyLib.Abstraction.BaseClass;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.NatNegotiation.Entity.Structure
{
    public class Client : ClientBase
    {
        public new UserInfo Info { get => (UserInfo)base.Info; private set => base.Info = value; }
        public new IUdpConnection Connection => (IUdpConnection)base.Connection;
        public Client(IConnection session) : base(session)
        {
            Info = new UserInfo(session.RemoteIPEndPoint);
        }
    }
}