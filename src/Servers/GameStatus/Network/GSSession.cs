using GameStatus.Entity.Structure.Misc;
using GameStatus.Handler.CmdSwitcher;
using UniSpyLib.Network;

namespace GameStatus.Network
{
    internal sealed class GSSession : UniSpyTCPSessionBase
    {

        public GSPlayerInfo PlayerData { get; set; }

        public GSSession(UniSpyTCPServerBase server) : base(server)
        {
            PlayerData = new GSPlayerInfo();
        }

        /// <summary>
        /// When client connect, we send our challenge first
        /// </summary>
        protected override void OnConnected() => SendAsync(GSConstants.ChallengeResponse);
        protected override void OnReceived(string message) => new GSCmdSwitcher(this, message).Switch();
    }
}
