using UniSpyServer.Servers.PresenceSearchPlayer.Handler;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.PresenceSearchPlayer.Entity.Structure
{
    public class Client : ClientBase
    {
        public Client(IConnection connection) : base(connection)
        {
            Info = new ClientInfo();
        }

        protected override ISwitcher CreateSwitcher(object buffer) => new CmdSwitcher(this, buffer);
    }
}