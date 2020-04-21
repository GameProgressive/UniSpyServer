using System.Linq;
using Chat.Entity.Structure.ChatChannel;
using Chat.Entity.Structure.ChatCommand;
using Chat.Handler.SystemHandler.ChannelManage;
using Chat.Server;
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


        public override void DataOperation()
        {
            base.DataOperation();

            ChatChannelManager.Channels.TryGetValue(_joinCmd.ChannelName, out _channel);
            if (_channel == null)
            {
                _channel = new ChatChannelBase();
                _channel.CreateChannel(_session, _cmd);
                ChatChannelManager.Channels.TryAdd(_joinCmd.ChannelName, _channel);
            }
            //channel.JoinChannel(_session);
            if (_channel.Property.ChannelMode.ModesKV['i'] == '+')
            {
                //invited only
                return;
            }
            if (_channel.Property.BanList.Where(c => c.Equals(_session)).Count() == 1)
            {
                return;
            }

            if (_channel.Property.ChannelUsers.Where(u => u.Equals(_session)).Count() != 0)
            {
                //already in channel
                return;
            }

            //first we send join information to all user in this channel
            MultiCaseJoin();

            _channel.AddBindOnUserAndChannel(_session);
            //send channel mode to joiner
            _channel.SendChannelModes(_session);
            //then we send user list which already in this channel ???????????
            _channel.SendChannelUsers(_session);

        }

        protected void MultiCaseJoin()
        {
            //first we send join information to all user in this channel
            string joinMessage =
            ChatCommandBase.BuildMessage(
           $"{_session.ClientInfo.NickName}!{_session.ClientInfo.UserName}@{ChatServer.ServerDomain}",
           "JOIN", $"{_channel.Property.ChannelName}");

            _channel.MultiCast(joinMessage);
        }

    }
}
