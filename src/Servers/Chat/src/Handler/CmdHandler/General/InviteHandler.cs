using UniSpy.Server.Chat.Abstraction.BaseClass;
using UniSpy.Server.Chat.Abstraction.Interface;
using UniSpy.Server.Chat.Aggregate;
using UniSpy.Server.Chat.Application;
using UniSpy.Server.Chat.Contract.Request.General;
using UniSpy.Server.Core.Abstraction.Interface;

namespace UniSpy.Server.Chat.Handler.CmdHandler.General
{
    public sealed class InviteHandler : CmdHandlerBase
    {
        private new InviteRequest _request => (InviteRequest)base._request;
        public InviteHandler(IChatClient client, InviteRequest request) : base(client, request)
        {
        }
        protected override void RequestCheck()
        {
            base.RequestCheck();
            if (!_client.Info.JoinedChannels.ContainsKey(_request.ChannelName))
            {
                throw new Chat.Exception("To invite user, you must in the channel.");
            }
            var chan = ChannelManager.GetChannel(_request.ChannelName);
            chan.Mode.InviteNickNames.Add(_request.NickName);
        }
    }
}