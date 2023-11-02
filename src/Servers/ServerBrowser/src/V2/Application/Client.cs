using UniSpy.Server.ServerBrowser.V2.Handler;
using UniSpy.Server.Core.Abstraction.BaseClass;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.ServerBrowser.V2.Aggregate.Misc;

namespace UniSpy.Server.ServerBrowser.V2.Application
{
    public sealed class Client : ClientBase
    {
        public new ClientInfo Info { get => (ClientInfo)base.Info; set => base.Info = value; }
        public Client(IConnection connection, IServer server) : base(connection, server)
        {
            // Crypto is init in ServerListHandler
            Info = new ClientInfo();
            IsLogRaw = true;
        }

        protected override ISwitcher CreateSwitcher(object buffer) => new CmdSwitcher(this, (byte[])buffer);
    }
}