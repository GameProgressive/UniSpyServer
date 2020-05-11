using System.Net;
using Chat.Entity.Structure;
using Chat.Entity.Structure.ChatChannel;
using Chat.Entity.Structure.ChatCommand;
using Chat.Entity.Structure.ChatResponse;
using Chat.Handler.SystemHandler.ChannelManage;
using Chat.Handler.SystemHandler.ChatSessionManage;
using Chat.Server;
using GameSpyLib.Common.Entity.Interface;

namespace Chat.Handler.CommandHandler.ChatBasicCommandHandler
{
    public class WHOHandler : ChatCommandHandlerBase
    {
        new WHO _cmd;

        public WHOHandler(ISession session, ChatCommandBase cmd) : base(session, cmd)
        {
            _cmd = (WHO)cmd;
        }

        public override void CheckRequest()
        {
            base.CheckRequest();

        }

        public override void DataOperation()
        {
            base.DataOperation();
            switch (_cmd.RequestType)
            {
                case WHOType.ChannelSearch:
                    ChannelSearch();
                    break;
                case WHOType.UserSearch:
                    UserSearch();
                    break;
            }
            AppendEndOfWhoRPL();
        }

        public override void ConstructResponse()
        {
            base.ConstructResponse();
        }

        private void ChannelSearch()
        {
            ChatChannelBase channel;
            if (!ChatChannelManager.GetChannel(_cmd.Name, out channel))
            {
                _errorCode = ChatError.IRCError;
                return;
            }
            _sendingBuffer = "";
            foreach (var user in channel.Property.ChannelUsers)
            {
                BuildWhoRPLForSingleUser("#unknown", user.Session);
            }
        }
        private void UserSearch()
        {
            ChatSession session;

            if (ChatSessionManager.GetSessionByNickName(_cmd.Name, out session))
            {
                BuildWhoRPLForSingleUser("#unknown", session);
            }
            else if (ChatSessionManager.GetSessionByUserName(_cmd.Name, out session))
            {
                BuildWhoRPLForSingleUser("#unknown", session);
            }
            else
            {
                _errorCode = ChatError.IRCError;
            }
        }

        private void AppendEndOfWhoRPL()
        {
            _sendingBuffer += ChatCommandBase.BuildReply(ChatReply.EndOfWho, $"* {_session.UserInfo.NickName} param3");
        }

        private string BuildWhoRPLForSingleUser(string channelName, ChatSession session)
        {
            string address = ((IPEndPoint)session.Socket.RemoteEndPoint).Address.ToString();
            return
                ChatCommandBase.BuildReply(
                    ChatReply.WhoReply,
                    $"* {channelName} " +
                    $"{session.UserInfo.UserName} {address} {session.UserInfo.NickName} param6 param7 param8");
        }
    }
}
