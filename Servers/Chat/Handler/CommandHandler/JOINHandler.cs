using System;
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
        public JOINHandler(IClient client, ChatCommandBase cmd) : base(client, cmd)
        {
            _joinCmd = (JOIN)cmd;
        }

        public override void DataOperation()
        {
            base.DataOperation();
            ChatChannelBase channel;
            ChatChannelManager.Channels.TryGetValue(_joinCmd.ChannelName, out channel);
            if (channel == null)
            {
                channel = new ChatChannelBase();
                channel.CreateChannel(_session,_cmd);
                ChatChannelManager.Channels.GetOrAdd(_joinCmd.ChannelName, channel);
            }
            channel.JoinChannel(_session);
        }

        public override void ConstructResponse()
        {
            base.ConstructResponse();
            _sendingBuffer = _joinCmd.BuildResponse(_session.ClientInfo.NickName, _session.ClientInfo.UserName);
            
            _sendingBuffer += ":irc.foonet.com 353 Pants = #istanbul :@Pants\r\n";
            _sendingBuffer += ":irc.foonet.com 366 Pants #istanbul :End of /NAMES list.\r\n";
        }
    }
}
