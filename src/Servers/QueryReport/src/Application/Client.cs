using UniSpyServer.Servers.QueryReport.V2.Entity.Structure.Redis;
using UniSpyServer.Servers.QueryReport.V2.Handler;
using UniSpyServer.Servers.QueryReport.V2.Handler.CmdHandler;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.QueryReport.Application
{
    public sealed class Client : ClientBase
    {
        public new ClientInfo Info { get => (ClientInfo)base.Info; set => base.Info = value; }
        public Client(IConnection connection) : base(connection)
        {
            IsLogRaw = true;
            // launch redis channel
            Info = new ClientInfo();
        }

        protected override ISwitcher CreateSwitcher(object buffer) => new CmdSwitcher(this, buffer);
    }
}