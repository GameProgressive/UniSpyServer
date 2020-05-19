using Chat.Entity.Structure.ChatCommand;
using Chat.Entity.Structure.ChatResponse;
using GameSpyLib.Common.Entity.Interface;

namespace Chat.Handler.CommandHandler
{

    public class UTMHandler : ChatMessageHandlerBase
    {
        new UTM _cmd;

        public UTMHandler(ISession client, ChatCommandBase cmd) : base(client, cmd)
        {
            _cmd = (UTM)cmd;
        }

        public override void DataOperation()
        {
            base.DataOperation();

            switch (_cmd.RequestType)
            {
                case ChatMessageType.ChannelMessage:
                    _sendingBuffer =
                        ChatReply.BuildUTMReply(
                            _user.UserInfo, _cmd.ChannelName, _cmd.Message);
                   // MultiCastUTM();
                    break;
                case ChatMessageType.UserMessage:
                    _sendingBuffer =
                        ChatReply.BuildUTMReply(
                        _otherSession.UserInfo, _cmd.NickName, _cmd.Message);
                    break;
            }
        }

        private void MultiCastUTM()
        {
            foreach (var user in _channel.Property.ChannelUsers)
            {
                string message = ChatReply.BuildUTMReply(
                            user.UserInfo, _cmd.ChannelName, _cmd.Message);
                user.Session.SendAsync(message);
            }
        }

    }
}
