using UniSpyServer.Servers.ServerBrowser.Abstraction;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.ServerBrowser.Entity
{
    public class Client : ClientBase
    {
        public string GameSecretKey { get; set; }
        public string ClientChallenge { get; set; }
        public AdHocRequest AdHocMessage { get; set; }
        public Client(ISession session, UserInfoBase userInfo) : base(session, userInfo)
        {
        }
    }
}