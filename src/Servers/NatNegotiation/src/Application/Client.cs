using System;
using UniSpy.Server.NatNegotiation.Handler;
using UniSpy.Server.Core.Abstraction.BaseClass;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Core.Extension;

namespace UniSpy.Server.NatNegotiation.Application
{
    public class Client : ClientBase
    {
        public new ClientInfo Info { get => (ClientInfo)base.Info; private set => base.Info = value; }
        public new IUdpConnection Connection => (IUdpConnection)base.Connection;
        public Client(IConnection connection, IServer server) : base(connection, server)
        {
            Info = new ClientInfo();
            IsLogRaw = true;
        }
        protected override ISwitcher CreateSwitcher(object buffer) => new CmdSwitcher(this, buffer);
        protected override void EventBinding()
        {
            ((IUdpConnection)Connection).OnReceive += OnReceived;
            _timer = new EasyTimer(TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(1), CheckExpiredClient);
        }
    }
}