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
        JOIN _joinCmd;
        ChatChannelBase _channel;
        ChatChannelUser _channelUser;
        public JOINHandler(IClient client, ChatCommandBase cmd) : base(client, cmd)
        {
            _joinCmd = (JOIN)cmd;
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
            ChatChannelManager.GetChannel(_joinCmd.ChannelName, out _channel);
        }

        public override void DataOperation()
        {
            base.DataOperation();

            _channelUser = new ChatChannelUser(_session);

            //if there do not have a channel we create it 
            if (_channel == null)
            {
                CreateChannel();
            }
            else
            {
                JoinChannel();
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
                    ChatCommandBase.BuildErrorRPL("",
                    _errorCode, $"* {_joinCmd.ChannelName}", "");
            }
        }

        public void CreateChannel()
        {
            _channel = new ChatChannelBase();
            _channelUser.SetCreatorProperties();
            _channel.CreateChannel(_channelUser, _joinCmd);
            ChatChannelManager.AddChannel(_joinCmd.ChannelName, _channel);
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

            if (_channel.IsUserBanned(_channelUser))
            {
                _errorCode = ChatError.BannedFromChan;
                return;
            }
            if (_channel.IsUserExisted(_channelUser))
            {
                _errorCode = ChatError.DataOperation;
                return;
            }
            //if all pass, it mean  we excute join channel
            _channelUser.SetDefaultProperties();
            _channel.JoinChannel(_channelUser);
        }
    }
}
