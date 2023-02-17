using UniSpy.Server.Chat.Abstraction.BaseClass;
using UniSpy.Server.Chat.Contract.Request.General;
using UniSpy.Server.Core.Abstraction.Interface;

namespace UniSpy.Server.Chat.Handler.CmdHandler.General
{
    
    public sealed class UserHandler : CmdHandlerBase
    {
        private new UserRequest _request => (UserRequest)base._request;
        public UserHandler(IClient client, IRequest request) : base(client, request){ }

        protected override void DataOperation()
        {
            _client.Info.UserName = _request.UserName;
            _client.Info.Name = _request.Name;
            _client.Info.IsLoggedIn = true;
        }
    }
}
