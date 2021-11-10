using UniSpyServer.Servers.GameStatus.Entity.Structure.Misc;
using UniSpyServer.Servers.GameStatus.Handler.CmdSwitcher;
using UniSpyServer.UniSpyLib.Encryption;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass.Network.Tcp.Server;
namespace UniSpyServer.Servers.GameStatus.Network
{
    public sealed class Session : UniSpyTcpSession
    {

        public GSPlayerInfo PlayerData { get; set; }

        public Session(UniSpyTcpServer server) : base(server)
        {
            PlayerData = new GSPlayerInfo();
        }

        /// <summary>
        /// When client connect, we send our challenge first
        /// </summary>
        protected override void OnConnected() => SendAsync(GSEncryption.Encrypt(UniSpyEncoding.GetBytes(GSConstants.ChallengeResponse)));
        protected override void OnReceived(string message) => new CmdSwitcher(this, message).Switch();

        protected override byte[] Decrypt(byte[] buffer) => GSEncryption.Decrypt(buffer);
        protected override byte[] Encrypt(byte[] buffer) => GSEncryption.Encrypt(buffer);

    }
}
