using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure;
using Chat.Entity.Structure.ChatCommand;
using Chat.Entity.Structure.Misc;
using Chat.Entity.Structure.Misc.ChannelInfo;
using Chat.Handler.SystemHandler.ChannelManage;
using System.Collections.Generic;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Extensions;

namespace Chat.Handler.CmdHandler.Channel
{
    /// <summary>
    /// Game will only join one channel at one time
    /// </summary>
    public class JOINHandler : ChatLogedInHandlerBase
    {
        protected new JOINRequest _request
        {
            get { return (JOINRequest)base._request; }
        }
        ChatChannel _channel;
        ChatChannelUser _user;
        public JOINHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
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
                _errorCode = ChatErrorCode.JoinedTooManyChannel;
                //_sendingBuffer =
                //    ChatIRCError.BuildToManyChannelError(_request.ChannelName);
                return;
            }
        }

        protected override void DataOperation()
        {
            base.DataOperation();
            _user = new ChatChannelUser(_session);
            if (ChatChannelManager.GetChannel(_request.ChannelName, out _channel))
            {
                //join
                JoinChannel();
            }
            else
            {
                //create
                CreateChannel();
            }
        }

        public void CreateChannel()
        {
            _channel = new ChatChannel();

            if (IsPeerServer(_request.ChannelName))
            {
                _channel.Property.IsPeerServer = true;
            }

            _user.SetDefaultProperties(true);

            _channel.Property.SetDefaultProperties(_user, _request);

            //simple check for avoiding program crash
            if (!_channel.IsUserExisted(_user))
            {
                _channel.AddBindOnUserAndChannel(_user);
            }

            //first we send join information to all user in this channel
            _channel.MultiCastJoin(_user);

            //then we send user list which already in this channel ???????????
            _channel.SendChannelUsersToJoiner(_user);

            //send channel mode to joiner
            _channel.SendChannelModesToJoiner(_user);

            ChatChannelManager.AddChannel(_request.ChannelName, _channel);
        }

        public void JoinChannel()
        {
            if (_session.UserInfo.IsJoinedChannel(_request.ChannelName))
            {
                //then we send user list which already in this channel ???????????
                _channel.SendChannelUsersToJoiner(_user);

                //send channel mode to joiner
                _channel.SendChannelModesToJoiner(_user);
            }
            else
            {
                //channel.JoinChannel(_session);
                if (_channel.Property.ChannelMode.IsInviteOnly)
                {
                    //invited only
                    _errorCode = ChatErrorCode.IRCError;
                    return;
                }

                if (_channel.IsUserBanned(_user))
                {
                    _errorCode = ChatErrorCode.IRCError;
                    _sendingBuffer = ChatIRCErrorCode.BuildBannedFromChannelError(_channel.Property.ChannelName);
                    return;
                }
                if (_channel.Property.ChannelUsers.Count >= _channel.Property.MaxNumberUser)
                {
                    _errorCode = ChatErrorCode.IRCError;
                    _sendingBuffer = ChatIRCErrorCode.BuildChannelIsFullError(_channel.Property.ChannelName);
                    return;
                }
                //if all pass, it mean  we excute join channel
                _user.SetDefaultProperties();


                //simple check for avoiding program crash
                if (_channel.IsUserExisted(_user))
                {
                    _errorCode = ChatErrorCode.UserAlreadyInChannel;
                    return;
                }

                _channel.AddBindOnUserAndChannel(_user);

                //first we send join information to all user in this channel
                _channel.MultiCastJoin(_user);

                //then we send user list which already in this channel ???????????
                _channel.SendChannelUsersToJoiner(_user);

                //send channel mode to joiner
                _channel.SendChannelModesToJoiner(_user);
            }
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
    }
}
