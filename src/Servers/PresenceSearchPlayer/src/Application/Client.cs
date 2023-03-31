using UniSpy.Server.PresenceSearchPlayer.Handler;
using UniSpy.Server.Core.Abstraction.BaseClass;
using UniSpy.Server.Core.Abstraction.Interface;

namespace UniSpy.Server.PresenceSearchPlayer.Application
{
    public sealed class Client : ClientBase
    {
        public Client(IConnection connection, IServer server) : base(connection, server)
        {
            Info = new ClientInfo();
        }

        protected override ISwitcher CreateSwitcher(object buffer) => new CmdSwitcher(this, buffer);
    }
}