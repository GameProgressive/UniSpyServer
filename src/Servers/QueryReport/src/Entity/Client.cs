using UniSpyServer.UniSpyLib.Abstraction.BaseClass;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.QueryReport.Entity
{
    public class Client : ClientBase
    {
        public Client(IConnection session) : base(session)
        {
            Info = new UserInfo(session.RemoteIPEndPoint);
        }
    }
}