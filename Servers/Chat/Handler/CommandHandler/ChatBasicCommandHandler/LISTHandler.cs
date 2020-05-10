using Chat.Entity.Structure.ChatChannel;
using Chat.Entity.Structure.ChatCommand;
using Chat.Entity.Structure.ChatResponse;
using Chat.Handler.SystemHandler.ChannelManage;
using GameSpyLib.Common.Entity.Interface;

namespace Chat.Handler.CommandHandler
{
    public class LISTHandler : ChatCommandHandlerBase
    {
        new LIST _cmd;
        //:irc.foonet.com 321 Pants Channel :Users  Name\r\n:irc.foonet.com 323 Pants :End of /LIST\r\n
        public LISTHandler(ISession client, ChatCommandBase cmd) : base(client, cmd)
        {
            _cmd = (LIST)cmd;
        }

        public override void DataOperation()
        {
            base.DataOperation();
            //add list response header

            _sendingBuffer = "";
            if (ChatChannelManager.Channels.Count != 0)
            {
                foreach (var channel in ChatChannelManager.Channels)
                {
                    //TODO
                    //add channel information here
                }
            }
            //add list response tail
            BuildEndOfListRPL();
        }

        public void BuildListRPL(ChatChannelBase channel)
        {
            _sendingBuffer = _session.UserInfo.BuildReply(ChatReply.ListStart,
                    $"{_session.UserInfo.NickName} Channel");
            //ChatCommandBase.BuildRPL(_session.UserInfo,
            //        ChatRPL.ListStart,
            //        $"{_session.UserInfo.NickName} Channel");
        }

        public void BuildEndOfListRPL()
        {
            _sendingBuffer += _session.UserInfo.BuildReply(ChatReply.ListEnd,
                     _session.UserInfo.NickName, "End of /LIST");
            //ChatCommandBase.BuildRPL(
            //        ChatRPL.ListEnd,
            //         _session.UserInfo.NickName, "End of /LIST");
        }

        public override void CheckRequest()
        {
            base.CheckRequest();

        }
    }
}
