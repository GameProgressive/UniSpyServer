using UniSpy.Server.Chat.Abstraction.BaseClass;
using UniSpy.Server.Chat.Error.IRC.General;
using UniSpy.Server.Chat.Contract.Request.Channel;
using UniSpy.Server.Chat.Contract.Request.General;
using UniSpy.Server.Chat.Contract.Response.Channel;
using UniSpy.Server.Chat.Contract.Result.Channel;
using UniSpy.Server.Chat.Abstraction.Interface;
using UniSpy.Server.Chat.Error.IRC.Channel;
using System;
using UniSpy.Server.Chat.Aggregate;

namespace UniSpy.Server.Chat.Handler.CmdHandler.Channel
{
    /// <summary>
    /// Game will only join one channel at one time
    /// </summary>

    public sealed class JoinHandler : ChannelHandlerBase
    {
        private new JoinRequest _request => (JoinRequest)base._request;
        private new JoinResult _result { get => (JoinResult)base._result; set => base._result = value; }
        private new JoinResponse _response { get => (JoinResponse)base._response; set => base._response = value; }
        public JoinHandler(IShareClient client, JoinRequest request) : base(client, request)
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
            if (!_client.Info.IsLoggedIn)
            {
                new Chat.Exception($"{_client.Info.NickName} Please login first!");
            }
            _request.Parse();
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
            // we acquire redis lock
            // redis lock
            var key = new Aggregate.Redis.ChannelCache
            {
                ChannelName = _request.ChannelName,
                GameName = _client.Info.GameName
            };
            using (var locker = new LinqToRedis.RedisLock(TimeSpan.FromSeconds(10), Application.StorageOperation.Persistance.ChannelCacheClient.Db, key))
            {
                if (locker.LockTake())
                {
                    _channel = Aggregate.Channel.GetLocalChannel(_request.ChannelName);
                    // if local channel is null
                    if (_channel is null)
                    {
                        _channel = Aggregate.Channel.GetChannelCache(key);
                    }
                    // if remote channel is null
                    if (_channel is null)
                    {
                        // create channel
                        _channel = Aggregate.Channel.CreateLocalChannel(_request.ChannelName, _client, _request.Password);
                        // we need to check whether this channel is gamespy official channel
                        switch (_channel.RoomType)
                        {
                            case PeerRoomType.Title:
                            case PeerRoomType.Group:
                                _user = _channel.AddUser(_client, _request.Password ?? null);
                                break;
                            case PeerRoomType.Normal:
                            case PeerRoomType.Staging:
                                _user = _channel.AddUser(_client, _request.Password ?? null, true, true);
                                break;
                        }
                    }
                    else
                    {
                        _user = _channel.AddUser(_client, _request.Password ?? null);
                    }
                    Aggregate.Channel.UpdateChannelCache(_user, _channel);
                }
                else
                {
                    throw new BadChannelKeyException("The channel is created by other person, try to re-join this channel", _request.ChannelName);
                }
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
                RequestType = ModeRequestType.GetChannelAndUserModes,
                ChannelName = _request.ChannelName,
                NickName = _user.Client.Info.NickName,
                UserName = _user.Client.Info.UserName,
                Password = _request.Password is null ? null : _request.Password
            };
            new ModeHandler(_client, userModeRequest).Handle();
        }
    }
}
