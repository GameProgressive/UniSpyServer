using System.Collections.Concurrent;
using UniSpy.Server.Chat.Abstraction.BaseClass;
using UniSpy.Server.Chat.Application;
using UniSpy.Server.Chat.Exception;
using UniSpy.Server.Chat.Exception.IRC.Channel;
using UniSpy.Server.Chat.Exception.IRC.General;
using UniSpy.Server.Chat.Aggregate.Misc;
using UniSpy.Server.Chat.Aggregate.Misc.ChannelInfo;
using UniSpy.Server.Chat.Contract.Request.Channel;
using UniSpy.Server.Chat.Contract.Request.General;
using UniSpy.Server.Chat.Contract.Response.Channel;
using UniSpy.Server.Chat.Contract.Result.Channel;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Chat.Aggregate.Redis.Contract;
using UniSpy.Server.Chat.Aggregate.Redis;

namespace UniSpy.Server.Chat.Handler.CmdHandler.Channel
{
    /// <summary>
    /// Game will only join one channel at one time
    /// </summary>

    public sealed class JoinHandler : LogedInHandlerBase
    {
        public static readonly ConcurrentDictionary<string, Chat.Aggregate.Misc.ChannelInfo.Channel> Channels = new ConcurrentDictionary<string, Chat.Aggregate.Misc.ChannelInfo.Channel>();
        private new JoinRequest _request => (JoinRequest)base._request;
        private new JoinResult _result { get => (JoinResult)base._result; set => base._result = value; }
        private new JoinResponse _response { get => (JoinResponse)base._response; set => base._response = value; }
        private static readonly GeneralMessageChannel GeneralMessageChannel = new GeneralMessageChannel();
        private Aggregate.Misc.ChannelInfo.Channel _channel;
        private ChannelUser _user;
        public JoinHandler(IClient client, IRequest request) : base(client, request)
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
                throw new ChatIRCTooManyChannels($"{_client.Info.NickName} is join too many channels");
            }
        }

        protected override void DataOperation()
        {
            _user = new ChannelUser(_client);
            // if (_connection.UserInfo.IsJoinedChannel(_request.ChannelName))
            // {
            //     // this is for not making game crash
            //     // if user is in this channel and we send the channel info back
            // }
            bool isChannelExist = Channels.ContainsKey(_request.ChannelName);
            if (isChannelExist)
            {
                _channel = Channels[_request.ChannelName];
                //join
                if (_client.Info.IsJoinedChannel(_request.ChannelName))
                {
                    // we do not send anything to this user and users in this channel
                    throw new ChatException($"User: {_user.Info.NickName} is already joined the channel: {_request.ChannelName}");
                }
                else
                {
                    if (_channel.Mode.IsInviteOnly)
                    {
                        //invited only
                        throw new IRCChannelException("This is an invited only channel.", IRCErrorCode.InviteOnlyChan, _request.ChannelName);
                    }
                    if (_channel.IsUserBanned(_user))
                    {
                        throw new IRCBannedFromChanException($"You are banned from this channel:{_request.ChannelName}.", _request.ChannelName);
                    }
                    if (_channel.Users.Count >= _channel.MaxNumberUser)
                    {
                        throw new IRCChannelIsFullException($"The channel:{_request.ChannelName} you are join is full.", _request.ChannelName);
                    }
                    //if all pass, it mean  we excute join channel
                    _user.SetDefaultProperties(false);
                    //simple check for avoiding program crash
                    if (_channel.IsUserExisted(_user))
                    {
                        throw new ChatException($"{_client.Info.NickName} is already in channel {_request.ChannelName}");
                    }
                    _channel.AddBindOnUserAndChannel(_user);
                }
            }
            else
            {
                //create
                if (StorageOperation.Persistance.IsPeerLobby(_request.ChannelName))
                {
                    _channel.IsPeerServer = true;
                    _channel = new Aggregate.Misc.ChannelInfo.Channel(_request.ChannelName, _user);
                    _user.SetDefaultProperties(false, false);
                }
                else
                {
                    _channel = new Aggregate.Misc.ChannelInfo.Channel(_request.ChannelName);
                    _user.SetDefaultProperties(true, true);
                }
                _channel.AddBindOnUserAndChannel(_user);
                Channels.TryAdd(_request.ChannelName, _channel);
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
                Password = _request.Password
            };
            new ModeHandler(_client, userModeRequest).Handle();
        }
    }
}
