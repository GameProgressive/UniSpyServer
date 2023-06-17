using UniSpy.Server.WebServer.Handler;
using UniSpy.Server.Core.Abstraction.BaseClass;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Core.Logging;

namespace UniSpy.Server.WebServer.Application
{
    public sealed class Client : ClientBase
    {
        public Client(IConnection connection, IServer server) : base(connection, server)
        {
            Info = new ClientInfo();
        }

        protected override ISwitcher CreateSwitcher(object buffer) => new CmdSwitcher(this, (IHttpRequest)buffer);

        protected override void OnReceived(object buffer)
        {
            var rq = (IHttpRequest)buffer;

            if (rq.Body.Length == 0 || rq.Body == "")
            {
                this.LogWarn($"ignore empty message");
                return;
            }
            base.OnReceived(buffer);
            if (!rq.KeepAlive)
                (Connection as IHttpConnection)?.Disconnect();
        }
    }
}