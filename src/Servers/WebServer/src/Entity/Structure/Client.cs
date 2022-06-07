using UniSpyServer.Servers.WebServer.Handler;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.WebServer.Entity.Structure
{
    public class Client : ClientBase
    {
        public Client(ISession session) : base(session)
        {
            Info = new ClientInfo();
        }

        protected override ISwitcher CreateSwitcher(object buffer) => new CmdSwitcher(this, buffer);

        protected override void OnReceived(object buffer) 
        {
            base.OnReceived(buffer);
            var rq = (IHttpRequest)buffer;
            if (!rq.KeepAlive)
                ((IHttpSession)Session).Disconnect();
        }
    }
}