using UniSpyServer.UniSpyLib.Abstraction.BaseClass;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.Chat.Entity
{
    public class Client : ClientBase
    {
        public Client(ISession session, UserInfoBase userInfo) : base(session, userInfo)
        {
        }
    }
}