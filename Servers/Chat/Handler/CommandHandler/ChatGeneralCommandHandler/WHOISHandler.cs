using System.Linq;
using Chat.Entity.Structure.ChatCommand;
using Chat.Entity.Structure.ChatResponse;
using Chat.Entity.Structure.ChatUser;
using Chat.Handler.SystemHandler.ChatSessionManage;
using GameSpyLib.Common.Entity.Interface;

namespace Chat.Handler.CommandHandler.ChatGeneralCommandHandler
{
    public class WHOISHandler : ChatCommandHandlerBase
    {
        new WHOISRequest _request;
        ChatUserInfo _userInfo;
        public WHOISHandler(ISession session, ChatRequestBase request) : base(session, request)
        {
            _request = new WHOISRequest(request.RawRequest);
        }

        protected override void CheckRequest()
        {
            base.CheckRequest();
            if (_errorCode != Entity.Structure.ChatError.NoError)
            {
                return;
            }

            var result = from s in ChatSessionManager.Sessions.Values
                         where s.UserInfo.NickName == _request.NickName
                         select s.UserInfo;

            if (result.Count() != 1)
            {
                _errorCode = Entity.Structure.ChatError.IRCError;
                _sendingBuffer = ChatIRCError.BuildNoSuchNickError();
                return;
            }
            _userInfo = result.FirstOrDefault();
        }

        protected override void DataOperation()
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
