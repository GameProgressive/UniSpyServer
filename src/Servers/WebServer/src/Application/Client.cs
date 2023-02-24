using UniSpy.Server.WebServer.Handler;
using UniSpy.Server.Core.Abstraction.BaseClass;
using UniSpy.Server.Core.Abstraction.Interface;

namespace UniSpy.Server.WebServer.Application
{
    public sealed class Client : ClientBase
    {
        public Client(IConnection connection) : base(connection)
        {
            Info = new ClientInfo();
        }

        protected override ISwitcher CreateSwitcher(object buffer) => new CmdSwitcher(this, (IHttpRequest)buffer);

        protected override void OnReceived(object buffer)
        {
            base.OnReceived(buffer);
            var rq = (IHttpRequest)buffer;
            if (!rq.KeepAlive)
                ((IHttpConnection)Connection).Disconnect();
        }
    }
}