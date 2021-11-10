using UniSpyServer.Servers.Chat.Abstraction.BaseClass;
using UniSpyServer.Servers.Chat.Entity.Contract;
using UniSpyServer.Servers.Chat.Entity.Structure.Misc.ChannelInfo;
using UniSpyServer.Servers.Chat.Entity.Structure.Request.General;
using UniSpyServer.Servers.Chat.Entity.Structure.Result.General;
using UniSpyServer.Servers.Chat.Entity.Structure.Request.Channel;
using UniSpyServer.Servers.Chat.Handler.CmdHandler.Channel;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.Chat.Handler.CmdHandler.General
{
    [HandlerContract("QUIT")]
    public sealed class QuitHandler : LogedInHandlerBase
    {
        private new QuitRequest _request => (QuitRequest)base._request;
        private new QuitResult _result
        {
            get => (QuitResult)base._result;
            set => base._result = value;
        }
        // when a user disconnected with server we can call this function
        public QuitHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _result = new QuitResult();
        }

        protected override void DataOperation()
        {
            foreach (var channel in _session.UserInfo.JoinedChannels)
            {
                ChannelUser user = channel.GetChannelUserBySession(_session);
                if (user == null)
                {
                    continue;
                }
                _result.ChannelInfos.Add(
                    new QuitDataModel
                    {
                        IsPeerServer = channel.Property.IsPeerServer,
                        IsChannelCreator = user.IsChannelCreator,
                        ChannelName = channel.Property.ChannelName,

                    });
            }
        }

        protected override void Response()
        {
            _response.Build();

            foreach (var channelInfo in _result.ChannelInfos)
            {
                // we create a PARTHandler to handle our quit request
                var partRequest = new PartRequest()
                {
                    ChannelName = channelInfo.ChannelName,
                    Reason = _request.Reason
                };
                new PartHandler(_session, partRequest).Handle();
            }
        }
    }
}
