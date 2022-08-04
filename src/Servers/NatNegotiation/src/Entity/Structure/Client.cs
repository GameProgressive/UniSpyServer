using System.Net;
using UniSpyServer.Servers.NatNegotiation.Handler;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Logging;

namespace UniSpyServer.Servers.NatNegotiation.Entity.Structure
{
    public class Client : ClientBase
    {
        public new ClientInfo Info { get => (ClientInfo)base.Info; private set => base.Info = value; }
        public new IUdpSession Session => (IUdpSession)base.Session;
        public Client(ISession session) : base(session)
        {
            Info = new ClientInfo();
        }

        protected override ISwitcher CreateSwitcher(object buffer) => new CmdSwitcher(this, buffer);

        protected override void LogNetworkReceiving(IPEndPoint ipEndPoint, object buffer) => LogWriter.LogNetworkReceiving(ipEndPoint, (byte[])buffer, true);
        protected override void LogNetworkSending(IPEndPoint ipEndPoint, object buffer) => LogWriter.LogNetworkSending(ipEndPoint, (byte[])buffer, true);
    }
}