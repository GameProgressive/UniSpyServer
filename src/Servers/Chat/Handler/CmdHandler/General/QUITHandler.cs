using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure.Misc.ChannelInfo;
using Chat.Entity.Structure.Request;
using Chat.Entity.Structure.Request.General;
using Chat.Entity.Structure.Result.General;
using Chat.Handler.CmdHandler.Channel;
using UniSpyLib.Abstraction.Interface;

namespace Chat.Handler.CmdHandler.General
{
    internal sealed class QUITHandler : LogedInHandlerBase
    {
        private new QUITRequest _request => (QUITRequest)base._request;
        private new QUITResult _result
        {
            get => (QUITResult)base._result;
            set => base._result = value;
        }
        // when a user disconnected with server we can call this function
        public QUITHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _result = new QUITResult();
        }

        protected override void DataOperation()
        {
            foreach (var channel in _session.UserInfo.JoinedChannels)
            {
                ChatChannelUser user = channel.GetChannelUserBySession(_session);
                if (user == null)
                {
                    continue;
                }
                _result.ChannelInfos.Add(
                    new QUITDataModel
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
                var partRequest = new PARTRequest()
                {
                    ChannelName = channelInfo.ChannelName,
                    Reason = _request.Reason
                };
                new PARTHandler(_session, partRequest).Handle();
            }
        }
    }
}
