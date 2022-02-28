using UniSpyServer.UniSpyLib.Abstraction.BaseClass;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.Chat.Entity.Structure
{
    public class Client : ClientBase
    {
        public new ClientInfo Info => (ClientInfo)base.Info;
        public new ITcpSession Session => (ITcpSession)base.Session;
        public Client(ISession session) : base(session)
        {
            base.Info = new ClientInfo(session.RemoteIPEndPoint);
        }

        //todo add ondisconnect event process
    }
}