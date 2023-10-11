using UniSpy.Server.Chat.Abstraction.BaseClass;
using UniSpy.Server.Chat.Contract.Request.General;
using UniSpy.Server.Chat.Contract.Response.Channel;
using UniSpy.Server.Chat.Contract.Result.Channel;
using UniSpy.Server.Chat.Abstraction.Interface;

namespace UniSpy.Server.Chat.Handler.CmdHandler.Channel
{

    public sealed class NamesHandler : ChannelHandlerBase
    {
        private new NamesRequest _request => (NamesRequest)base._request;
        private new NamesResult _result { get => (NamesResult)base._result; set => base._result = value; }
        public NamesHandler(IShareClient client, NamesRequest request) : base(client, request) { }
        public NamesHandler(IShareClient client, NamesRequest request, Aggregate.Channel channel, Aggregate.ChannelUser user) : base(client, request)
        {
            _user = user;
            _channel = channel;
        }

        protected override void DataOperation()
        {
            _result = new NamesResult();
            _result.AllChannelUserNicks = _channel.GetAllUsersNickString();
            _result.ChannelName = _channel.Name;
            _result.RequesterNickName = _user.Client.Info.NickName;
        }

        /// <summary>
        /// We do not publish message in names handler
        /// </summary>
        protected override void PublishMessage() { }
        protected override void ResponseConstruct()
        {
            _response = new NamesResponse(_request, _result);
        }
    }
}
