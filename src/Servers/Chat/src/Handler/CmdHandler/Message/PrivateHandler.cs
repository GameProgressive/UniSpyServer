using UniSpy.Server.Chat.Abstraction.BaseClass;
using UniSpy.Server.Chat.Abstraction.Interface;
using UniSpy.Server.Chat.Application;
using UniSpy.Server.Chat.Contract.Request.Message;
using UniSpy.Server.Chat.Contract.Response.Message;
using UniSpy.Server.Chat.Contract.Result.Message;

namespace UniSpy.Server.Chat.Handler.CmdHandler.Message
{

    public sealed class PrivateHandler : MessageHandlerBase
    {
        private new PrivateRequest _request => (PrivateRequest)base._request;
        private new PrivateResult _result { get => (PrivateResult)base._result; set => base._result = value; }
        public PrivateHandler(IShareClient client, PrivateRequest request) : base(client, request)
        {
            _result = new PrivateResult();
        }
        protected override void ChannelMessageDataOpration()
        {
            var data = ClientManager.ClientPool;
            // if (_channel.Mode.IsModeratedChannel)
            // {
            //     return;
            // }

            if (_channel.IsUserBanned(_user))
            {
                return;
            }

            if (!_user.IsVoiceable)
            {
                return;
            }
            // if (_user.Client.Info.IsQuietMode)
            // {
            //     return;
            // }
            _result.IsBroadcastMessage = true;
        }
        protected override void ResponseConstruct()
        {
            _response = new PrivateResponse(_request, _result);
        }
    }
}
