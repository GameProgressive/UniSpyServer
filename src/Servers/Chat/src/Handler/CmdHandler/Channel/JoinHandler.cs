using UniSpy.Server.Chat.Abstraction.BaseClass;
using UniSpy.Server.Chat.Error.IRC.General;
using UniSpy.Server.Chat.Aggregate;
using UniSpy.Server.Chat.Contract.Request.Channel;
using UniSpy.Server.Chat.Contract.Request.General;
using UniSpy.Server.Chat.Contract.Response.Channel;
using UniSpy.Server.Chat.Contract.Result.Channel;
using UniSpy.Server.Chat.Aggregate.Redis;
using UniSpy.Server.Chat.Abstraction.Interface;
using UniSpy.Server.Chat.Error.IRC.Channel;

namespace UniSpy.Server.Chat.Handler.CmdHandler.Channel
{
    /// <summary>
    /// Game will only join one channel at one time
    /// </summary>

    public sealed class JoinHandler : LogedInHandlerBase
    {
        private new JoinRequest _request => (JoinRequest)base._request;
        private new JoinResult _result { get => (JoinResult)base._result; set => base._result = value; }
        private new JoinResponse _response { get => (JoinResponse)base._response; set => base._response = value; }
        private static readonly GeneralMessageChannel GeneralMessageChannel = new GeneralMessageChannel();
        private Aggregate.Channel _channel;
        private ChannelUser _user;
        public JoinHandler(IChatClient client, JoinRequest request) : base(client, request)
        {
            _result = new JoinResult();
        }

        //1.筛选出所要加入的频道，如果不存在则创建(select the channel that user want to join, if channel does not exist creating it)
        //2.检查用户名nickname是否已经在频道中存在(check if user's nickname existed in channel)
        //若存在则提醒用户名字冲突
        //不存在则加入频道
        //广播加入信息
        //发送频道模式给此用户
        //发送频道用户列表给此用户

        protected override void RequestCheck()
        {
            base.RequestCheck();
            //some GameSpy game only allow one player join one chat room
            //but GameSpy Arcade can join more than one channel
            if (_client.Info.JoinedChannels.Count > 3)
            {
                throw new TooManyChannels($"{_client.Info.NickName} is join too many channels");
            }
            if (_client.Info.GameName == null)
            {
                throw new NoSuchChannelException("Game name is required for join a channel", _request.ChannelName);
            }
        }

        protected override void DataOperation()
        {
            lock (ChannelManager.Channels)
            {
                var isChannelExistOnLocal = ChannelManager.IsChannelExist(_request.ChannelName);
                if (isChannelExistOnLocal)
                {
                    _channel = ChannelManager.GetChannel(_request.ChannelName);
                    _user = _channel.AddUser(_client, _request.Password ?? null);
                }
                else
                {
                    // create channel
                    _channel = ChannelManager.CreateChannel(_request.ChannelName, _request.Password ?? null, _client);
                    _user = _channel.AddUser(_client, _request.Password ?? null, true, true);
                }
                Aggregate.Channel.UpdateChannelCache(_user);
                // Aggregate.Channel.UpdatePeerRoomInfo(_user);
            }
            _result.AllChannelUserNicks = _channel.GetAllUsersNickString();
            _result.JoinerNickName = _client.Info.NickName;
            _result.ChannelModes = _channel.Mode.ToString();
            _result.JoinerPrefix = _client.Info.IRCPrefix;
        }

        protected override void ResponseConstruct()
        {
            _response = new JoinResponse(_request, _result);
        }

        protected override void Response()
        {
            // base.Response();
            if (_response is null)
            {
                return;
            }

            //first we send join information to all user in this channel
            _channel.MultiCast(_client, _response);

            var namesRequest = new NamesRequest
            {
                ChannelName = _request.ChannelName
            };
            new NamesHandler(_client, namesRequest).Handle();

            var userModeRequest = new ModeRequest
            {
                RequestType = ModeRequestType.GetChannelUserModes,
                ChannelName = _request.ChannelName,
                NickName = _user.Info.NickName,
                UserName = _user.Info.UserName,
                Password = _request.Password is null ? null : _request.Password
            };
            new ModeHandler(_client, userModeRequest).Handle();
        }
    }
}
