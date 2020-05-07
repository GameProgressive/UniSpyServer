using Chat.Entity.Structure;
using Chat.Entity.Structure.ChatChannel;
using Chat.Entity.Structure.ChatCommand;
using Chat.Handler.SystemHandler.ChannelManage;
using GameSpyLib.Common.Entity.Interface;

namespace Chat.Handler.CommandHandler
{
    /// <summary>
    /// Game will only join one channel at one time
    /// </summary>
    public class JOINHandler : ChatCommandHandlerBase
    {
        new JOIN _cmd;
        ChatChannelBase _channel;
        ChatChannelUser _user;
        public JOINHandler(ISession client, ChatCommandBase cmd) : base(client, cmd)
        {
            _cmd = (JOIN)cmd;
        }

        //1.筛选出所要加入的频道，如果不存在则创建

        //2.检查用户名nickname是否已经在频道中存在
        //若存在则提醒用户名字冲突
        //不存在则加入频道
        //广播加入信息
        //发送频道模式给此用户
        //发送频道用户列表给此用户
        //_errorCode>1024
        public override void CheckRequest()
        {
            base.CheckRequest();
            
        }

        public override void DataOperation()
        {
            base.DataOperation();

            _user = new ChatChannelUser(_session);

            if (ChatChannelManager.GetChannel(_cmd.ChannelName, out _channel))
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

        public override void ConstructResponse()
        {
            base.ConstructResponse();
            //TODO
            //we construct error response here
            if (_errorCode > ChatError.NoError)
            {
                _sendingBuffer =
                    ChatCommandBase.BuildNumericErrorRPL("",
                    _errorCode, $"* {_cmd.ChannelName}", "");
            }
        }

        public void CreateChannel()
        {
            _channel = new ChatChannelBase();
            _user.SetCreatorProperties();
            _channel.Property.SetDefaultProperties(_user, _cmd);

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

            ChatChannelManager.AddChannel(_cmd.ChannelName, _channel);
        }

        public void JoinChannel()
        {
            //channel.JoinChannel(_session);
            if (_channel.Property.ChannelMode.IsInviteOnly)
            {
                //invited only
                _errorCode = ChatError.InviteOnlyChan;
                return;
            }

            if (_channel.IsUserBanned(_user))
            {
                _errorCode = ChatError.BannedFromChan;
                return;
            }
            //if there are no players in this channel we set first joined player as creator and operator
            if (_channel.Property.ChannelUsers.Count == 0)
            {
                _user.SetCreatorProperties();
            }
            else
            {
                //if all pass, it mean  we excute join channel
                _user.SetDefaultProperties();
            }

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
        }
    }
}
