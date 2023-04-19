using UniSpy.Server.Core.Abstraction.BaseClass;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Core.Encryption;
using UniSpy.Server.Core.Logging;
using UniSpy.Server.ServerBrowser.V1.Handler;

namespace UniSpy.Server.ServerBrowser.V1.Application
{
    public sealed class Client : ClientBase
    {
        public new ClientInfo Info { get => (ClientInfo)base.Info; private set => base.Info = value; }
        public Client(IConnection connection, IServer server) : base(connection, server)
        {
            Info = new ClientInfo();
        }
        protected override void OnConnected()
        {
            this.LogNetworkSending(ClientInfo.ChallengeResponse);
            Connection.Send(ClientInfo.ChallengeResponse);
            base.OnConnected();
        }

        protected override ISwitcher CreateSwitcher(object buffer) => new CmdSwitcher(this, UniSpyEncoding.GetString((byte[])buffer));
    }
}