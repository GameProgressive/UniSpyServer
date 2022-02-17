using UniSpyServer.Servers.Chat.Entity.Structure.Misc;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.Chat.Entity
{
    public class Client : ClientBase
    {
        public new UserInfo UserInfo => (UserInfo)base.Info;
        public new ITcpConnection Session => (ITcpConnection)base.Connection;
        public Client(IConnection session) : base(session)
        {
            base.Info = new UserInfo(session.RemoteIPEndPoint);
        }
    }
}