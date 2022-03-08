using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Logging;

namespace UniSpyServer.Servers.Chat.Abstraction.BaseClass
{
    public abstract class LogedInHandlerBase : CmdHandlerBase
    {
        public LogedInHandlerBase(IClient client, IRequest request) : base(client, request){ }

        public override void Handle()
        {
            if (!_client.Info.IsLoggedIn)
            {
                LogWriter.Info("Please login first!");
                return;
            }

            base.Handle();
        }
    }
}
