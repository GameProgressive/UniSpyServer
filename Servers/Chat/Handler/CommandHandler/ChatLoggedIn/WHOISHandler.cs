using System.Linq;
using Chat.Entity.Structure.ChatCommand;
using Chat.Entity.Structure.ChatResponse;
using Chat.Entity.Structure.ChatUser;
using Chat.Handler.SystemHandler.ChatSessionManage;
using GameSpyLib.Common.Entity.Interface;

namespace Chat.Handler.CommandHandler
{
    public class WHOISHandler : ChatCommandHandlerBase
    {
        new WHOIS _cmd;
        ChatUserInfo _userInfo;
        public WHOISHandler(ISession client, ChatCommandBase cmd) : base(client, cmd)
        {
            _cmd = (WHOIS)cmd;
        }

        public override void CheckRequest()
        {
            base.CheckRequest();
            if (_errorCode != Entity.Structure.ChatError.NoError)
            {
                return;
            }

            var result = from s in ChatSessionManager.Sessions.Values
                         where s.UserInfo.NickName == _cmd.NickName
                         select s.UserInfo;

            if (result.Count() != 1)
            {
                _errorCode = Entity.Structure.ChatError.IRCError;
                _sendingBuffer = ChatIRCError.BuildNoSuchNickError();
                return;
            }
            _userInfo = result.FirstOrDefault();
        }

        public override void DataOperation()
        {
            base.DataOperation();

            _sendingBuffer =
                ChatReply.BuildWhoIsUserReply(_userInfo);

            BuildJoinedChannelReply();

            _sendingBuffer +=
                ChatReply.BuildEndOfWhoIsReply(_userInfo);
        }

        private void BuildJoinedChannelReply()
        {
            if (_userInfo.JoinedChannels.Count() != 0)
            {
                string channelNames = "";
                //todo remove last space
                foreach (var channel in _userInfo.JoinedChannels)
                {
                    channelNames += $"@{channel.Property.ChannelName} ";
                }

                _sendingBuffer +=
                    ChatReply.BuildWhoIsChannelReply(_userInfo, channelNames);

            }
        }
    }
}
