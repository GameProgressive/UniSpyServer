using UniSpy.Server.Core.Abstraction.BaseClass;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.QueryReport.V2.Handler;

namespace UniSpy.Server.QueryReport.Application
{
    public sealed class Client : ClientBase
    {
        public Client(IConnection connection, IServer server) : base(connection, server)
        {
            IsLogRaw = true;
            // launch redis channel
            Info = new ClientInfo();
        }

        public new ClientInfo Info { get => (ClientInfo)base.Info; private set => base.Info = value; }


        protected override ISwitcher CreateSwitcher(object buffer) => new CmdSwitcher(this, (byte[])buffer);
    }
}