using UniSpyServer.UniSpyLib.Abstraction.BaseClass;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Encryption;

namespace UniSpyServer.Servers.CDKey.Entity.Structure
{
    public class Client : ClientBase
    {
        public Client(ISession session) : base(session)
        {
            Crypto = new XOREncoding(XorType.Type0);
        }

        protected override void OnReceived(object buffer)
        {
            base.OnReceived(buffer);
            var newBuffer = Crypto.Decrypt((byte[])buffer);
        }
    }
}