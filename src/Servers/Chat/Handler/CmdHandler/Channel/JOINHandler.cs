using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure;
using Chat.Entity.Structure.Misc;
using Chat.Entity.Structure.Misc.ChannelInfo;
using Chat.Entity.Structure.Request;
using Chat.Entity.Structure.Response.General;
using Chat.Entity.Structure.Result.Channel;
using Chat.Handler.SystemHandler.ChannelManage;
using System.Collections.Generic;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Extensions;

namespace Chat.Handler.CmdHandler.Channel
{
    /// <summary>
    /// Game will only join one channel at one time
    /// </summary>
    internal sealed class JOINHandler : ChatLogedInHandlerBase
    {
        private new JOINRequest _request => (JOINRequest)base._request;
        private new JOINResult _result
        {
            get => (JOINResult)base._result;
            set => base._result = value;
        }
        private new JOINResponse _response
        {
            get => (JOINResponse)base._response;
            set => base._response = value;
        }
        ChatChannel _channel;
        ChatChannelUser _user;
        public JOINHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _result = new JOINResult();
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
            if (_session.UserInfo.JoinedChannels.Count > 3)
            {
                _result.ErrorCode = ChatErrorCode.IRCError;
                _result.IRCErrorCode = ChatIRCErrorCode.TooManyChannels;
                return;
            }
        }

        protected override void DataOperation()
        {
            _user = new ChatChannelUser(_session.UserInfo);
            if (ChatChannelManager.GetChannel(_request.ChannelName, out _channel))
            {
                _result.IsCreateChannel = true;
                //join
                if (_session.UserInfo.IsJoinedChannel(_request.ChannelName))
                {
                    _result.IsAlreadyJoinedChannel = true;
                }
                else
                {
                    if (_channel.Property.ChannelMode.IsInviteOnly)
                    {
                        //invited only
                        _result.ErrorCode = ChatErrorCode.IRCError;
                        return;
                    }
                    if (_channel.IsUserBanned(_user))
                    {
                        _result.ErrorCode = ChatErrorCode.IRCError;
                        _result.IRCErrorCode = ChatIRCErrorCode.BannedFromChan;
                        return;
                    }
                    if (_channel.Property.ChannelUsers.Count >= _channel.Property.MaxNumberUser)
                    {
                        _result.ErrorCode = ChatErrorCode.IRCError;
                        _result.IRCErrorCode = ChatIRCErrorCode.ChannelIsFull;
                        return;
                    }
                    //if all pass, it mean  we excute join channel
                    _user.SetDefaultProperties(false);
                    //simple check for avoiding program crash
                    if (_channel.IsUserExisted(_user))
                    {
                        _result.ErrorCode = ChatErrorCode.UserAlreadyInChannel;
                        return;
                    }
                    _channel.AddBindOnUserAndChannel(_user);

                }
            }
            else
            {
                //create
                _channel = new ChatChannel();
                if (IsPeerServer(_request.ChannelName))
                {
                    _channel.Property.IsPeerServer = true;
                }
                _user.SetDefaultProperties(true);
                _channel.Property.SetDefaultProperties(_user, _request);
                _channel.AddBindOnUserAndChannel(_user);
                ChatChannelManager.AddChannel(_request.ChannelName, _channel);
            }

            _result.ChannelUserNicks = _channel.GetAllUsersNickString();
            _result.JoinerNickName = _session.UserInfo.NickName;
            _result.ChannelModes = _channel.Property.ChannelMode.GetChannelMode();
            _result.JoinerPrefix = _session.UserInfo.IRCPrefix;
        }

        private bool IsPeerServer(string name)
        {
            string[] buffer = name.Split('!', System.StringSplitOptions.RemoveEmptyEntries);
            if (buffer.Length != 3)
            {
                return false;
            }

            List<string> peerGameKeys = RedisExtensions.GetAllKeys(RedisDBNumber.PeerGroup);
            if (buffer[2].Length > 2 && peerGameKeys.Contains(buffer[1]))
            {
                return true;
            }
            return false;
        }

        protected override void ResponseConstruct()
        {
            _response = new JOINResponse(_request, _result);
        }

        protected override void Response()
        {
            if (_response == null)
            {
                return;
            }
            _response.Build();
            if (!_result.IsAlreadyJoinedChannel)
            {
                //first we send join information to all user in this channel
                _channel.MultiCast(_response.SendingBufferOfChannelUsers);
            }
            //TODO checkout whether need send mode in another single response
            _session.SendAsync(_response.SendingBuffer);
            // _session.SendAsync(_response.ModeReply);
        }
    }
}
