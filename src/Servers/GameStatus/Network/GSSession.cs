using GameStatus.Entity.Structure.Misc;
using GameStatus.Handler.CmdSwitcher;
using UniSpyLib.Encryption;
using UniSpyLib.Abstraction.BaseClass.Network.Tcp.Server;
namespace GameStatus.Network
{
    internal sealed class GSSession : UniSpyTcpSession
    {

        public GSPlayerInfo PlayerData { get; set; }

        public GSSession(UniSpyTcpServer server) : base(server)
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
