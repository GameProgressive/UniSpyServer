using UniSpyServer.Servers.GameTrafficRelay.Entity.Structure;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.GameTrafficRelay.Test
{
    class TestClient : Client
    {
        public TestClient(ISession session) : base(session)
        {
        }

        public void OnReceived(byte[] buffer)
        {
            base.OnReceived(buffer);
        }
    }
}