using System.Net;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;

namespace UniSpyServer.Servers.GameStatus.Entity
{
    public class ClientInfo : ClientInfoBase
    {
        public const string ChallengeResponse = @"\challenge\00000000000000000000\final\";
        public int? SessionKey;
        public string GameName;
        public ClientInfo(IPEndPoint remoteIPEndPoint) : base(remoteIPEndPoint)
        {
        }
    }
}