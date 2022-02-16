using UniSpyServer.Servers.Chat.Entity.Structure.Misc;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.Chat.Entity
{
    public class Client : ClientBase
    {
        public new UserInfo UserInfo => (UserInfo)base.UserInfo;
        public Client(ISession session, UserInfoBase userInfo) : base(session, userInfo)
        {
        }
    }
}