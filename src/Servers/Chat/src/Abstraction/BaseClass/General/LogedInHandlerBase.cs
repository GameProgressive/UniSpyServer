using UniSpy.Server.Chat.Abstraction.Interface;
using UniSpy.Server.Core.Abstraction.Interface;

namespace UniSpy.Server.Chat.Abstraction.BaseClass
{
    public abstract class LogedInHandlerBase : CmdHandlerBase
    {
        public LogedInHandlerBase(IShareClient client, IRequest request) : base(client, request) { }

        // public override void Handle()
        // {
        //     if (!_client.Info.IsLoggedIn)
        //     {
        //         _client.LogInfo($"{_client.Info.NickName} Please login first!");
        //         return;
        //     }

        //     base.Handle();
        // }
        protected override void RequestCheck()
        {
            if (!_client.Info.IsLoggedIn)
            {
                new Chat.Exception($"{_client.Info.NickName} Please login first!");
            }
            base.RequestCheck();
        }
    }
}
