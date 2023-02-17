using UniSpy.Server.CDKey.Handler;
using UniSpy.Server.Core.Abstraction.BaseClass;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Core.Encryption;

namespace UniSpy.Server.CDKey.Application
{
    internal sealed class Client : ClientBase
    {
        public Client(IConnection connection) : base(connection)
        {
            Crypto = new XOREncoding(XorType.Type0);
        }

        protected override ISwitcher CreateSwitcher(object buffer) => new CmdSwitcher(this, buffer);

        protected override void OnReceived(object buffer)
        {
            base.OnReceived(buffer);
            var newBuffer = Crypto.Decrypt((byte[])buffer);
        }
    }
}