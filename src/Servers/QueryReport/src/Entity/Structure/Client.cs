using UniSpyServer.Servers.QueryReport.Entity.Structure.Redis;
using UniSpyServer.Servers.QueryReport.Handler;
using UniSpyServer.Servers.QueryReport.Handler.CmdHandler;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.QueryReport.Entity.Structure
{
    public class Client : ClientBase
    {
        public new ClientInfo Info { get => (ClientInfo)base.Info; set => base.Info = value; }
        public Client(ISession session) : base(session)
        {
            // launch redis channel
            if (ClientMessageHandler.Channel is null)
            {
                ClientMessageHandler.Channel = new RedisChannel();
                ClientMessageHandler.Channel.StartSubscribe();
            }
            Info = new ClientInfo();
        }

        protected override ISwitcher CreateSwitcher(object buffer) => new CmdSwitcher(this, buffer);

    }
}