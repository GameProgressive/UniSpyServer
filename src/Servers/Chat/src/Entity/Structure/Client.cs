using UniSpyServer.Servers.Chat.Entity.Structure.Misc;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.Chat.Entity
{
    public class Client : ClientBase
    {
        public new UserInfo Info => (UserInfo)base.Info;
        public new ITcpSession Session => (ITcpSession)base.Session;
        public Client(ISession session) : base(session)
        {
            base.Info = new UserInfo(session.RemoteIPEndPoint);
        }
    }
}