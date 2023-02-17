using UniSpy.Server.Chat.Abstraction.BaseClass;
using UniSpy.Server.Chat.Aggregate.Misc.ChannelInfo;
using UniSpy.Server.Chat.Contract.Request.Channel;
using UniSpy.Server.Chat.Contract.Request.General;
using UniSpy.Server.Chat.Handler.CmdHandler.Channel;
using UniSpy.Server.Core.Abstraction.Interface;

namespace UniSpy.Server.Chat.Handler.CmdHandler.General
{

    public sealed class QuitHandler : LogedInHandlerBase
    {
        private new QuitRequest _request => (QuitRequest)base._request;
        // when a user disconnected with server we can call this function
        public QuitHandler(IClient client, IRequest request) : base(client, request) { }
        protected override void RequestCheck()
        {
            if (_request.RawRequest is null)
            {
                return;
            }
            base.RequestCheck();
        }

        protected override void DataOperation()
        {
            foreach (var channel in _client.Info.JoinedChannels.Values)
            {
                ChannelUser user = channel.GetChannelUser(_client);
                if (user is null)
                {
                    continue;
                }
                // we create a PARTHandler to handle our quit request
                var partRequest = new PartRequest()
                {
                    ChannelName = channel.Name,
                    Reason = _request.Reason
                };
                new PartHandler(_client, partRequest).Handle();
                // client is loged out
                _client.Info.IsLoggedIn = false;
            }
        }
    }
}
