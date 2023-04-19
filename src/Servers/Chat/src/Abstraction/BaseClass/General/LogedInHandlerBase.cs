using UniSpy.Server.Chat.Abstraction.Interface;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Core.Logging;

namespace UniSpy.Server.Chat.Abstraction.BaseClass
{
    public abstract class LogedInHandlerBase : CmdHandlerBase
    {
        public LogedInHandlerBase(IChatClient client, IRequest request) : base(client, request) { }

        public override void Handle()
        {
            if (!_client.Info.IsLoggedIn)
            {
                _client.LogInfo($"{_client.Info.NickName} Please login first!");
                return;
            }

            base.Handle();
        }
    }
}
