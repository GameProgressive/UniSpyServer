using System;
using UniSpyServer.Servers.NatNegotiation.Handler;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Extensions;

namespace UniSpyServer.Servers.NatNegotiation.Application
{
    public class Client : ClientBase
    {
        public new ClientInfo Info { get => (ClientInfo)base.Info; private set => base.Info = value; }
        public new IUdpConnection Connection => (IUdpConnection)base.Connection;
        public Client(IConnection connection) : base(connection)
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