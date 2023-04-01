using UniSpy.Server.ServerBrowser.Handler;
using UniSpy.Server.Core.Abstraction.BaseClass;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.ServerBrowser.Aggregate.Misc;

namespace UniSpy.Server.ServerBrowser.Application
{
    public sealed class Client : ClientBase
    {
        public new ClientInfo Info { get => (ClientInfo)base.Info; set => base.Info = value; }
        private BufferCache _bufferCache = new BufferCache();
        public Client(IConnection connection, IServer server) : base(connection, server)
        {
            // Crypto is init in ServerListHandler
            Info = new ClientInfo();
            IsLogRaw = true;
        }

        protected override ISwitcher CreateSwitcher(object buffer) => new CmdSwitcher(this, buffer);

        protected override void OnReceived(object buffer)
        {
            if (_bufferCache.ProcessBuffer((byte[])buffer, out var completeBuffer))
            {
                base.OnReceived(completeBuffer);
            }
        }
    }
}