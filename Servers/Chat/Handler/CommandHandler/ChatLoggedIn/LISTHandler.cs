using Chat.Entity.Structure.ChatChannel;
using Chat.Entity.Structure.ChatCommand;
using Chat.Entity.Structure.ChatResponse;
using Chat.Handler.SystemHandler.ChannelManage;
using GameSpyLib.Common.Entity.Interface;

namespace Chat.Handler.CommandHandler
{
    //todo unfinished
    public class LISTHandler : ChatLogedInHandlerBase
    {
        new LIST _request;
        //:irc.foonet.com 321 Pants Channel :Users  Name\r\n:irc.foonet.com 323 Pants :End of /LIST\r\n
        public LISTHandler(ISession session, ChatRequestBase request) : base(session, request)
        {
            _request = new LIST(request.CmdName);
        }

        protected override void DataOperation()
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
            _sendingBuffer = ChatReply.BuildListStartReply(_session.UserInfo, channel.Property);
        }

        public void BuildEndOfListRPL()
        {
            _sendingBuffer += ChatReply.BuildListEndReply(_session.UserInfo);
        }

    }
}
