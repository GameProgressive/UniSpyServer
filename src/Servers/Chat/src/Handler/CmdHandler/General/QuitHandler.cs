using UniSpyServer.Servers.Chat.Abstraction.BaseClass;
using UniSpyServer.Servers.Chat.Entity.Structure.Misc.ChannelInfo;
using UniSpyServer.Servers.Chat.Entity.Structure.Request.Channel;
using UniSpyServer.Servers.Chat.Entity.Structure.Request.General;
using UniSpyServer.Servers.Chat.Handler.CmdHandler.Channel;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.Chat.Handler.CmdHandler.General
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
