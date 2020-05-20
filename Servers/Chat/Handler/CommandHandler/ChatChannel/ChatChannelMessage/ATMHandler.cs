using System;
using Chat.Entity.Structure.ChatChannel;
using Chat.Entity.Structure.ChatCommand;
using Chat.Entity.Structure.ChatResponse;
using GameSpyLib.Common.Entity.Interface;

namespace Chat.Handler.CommandHandler
{
    public class ATMHandler : ChatMessageHandlerBase
    {
        new ATM _cmd;

        public ATMHandler(ISession session, ChatCommandBase cmd) : base(session, cmd)
        {
            _cmd = (ATM)cmd;
        }

        public override void DataOperation()
        {
            base.DataOperation();
            switch (_cmd.RequestType)
            {
                case ChatMessageType.ChannelMessage:
                    _sendingBuffer =
                        ChatReply.BuildATMReply(
                        _user.UserInfo, _cmd.ChannelName, _cmd.Message);
                    break;
                case ChatMessageType.UserMessage:
                    _sendingBuffer =
                        ChatReply.BuildATMReply(
                        _session.UserInfo, _cmd.NickName, _cmd.Message);
                    break;
            }
        }
    }
}
