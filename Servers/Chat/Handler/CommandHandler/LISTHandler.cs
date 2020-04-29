using System;
using Chat.Entity.Structure;
using Chat.Entity.Structure.ChatCommand;
using Chat.Handler.SystemHandler.ChannelManage;
using Chat.Server;
using GameSpyLib.Common.Entity.Interface;

namespace Chat.Handler.CommandHandler
{
    public class LISTHandler:ChatCommandHandlerBase
    {
        LIST _listCmd;
        //:irc.foonet.com 321 Pants Channel :Users  Name\r\n:irc.foonet.com 323 Pants :End of /LIST\r\n
        public LISTHandler(ISession client, ChatCommandBase cmd) : base(client, cmd)
        {
            _listCmd = (LIST)cmd;
        }

        public override void DataOperation()
        {
            base.DataOperation();
            //add list response header
            _sendingBuffer = ChatCommandBase.BuildNormalRPL(
                   ChatServer.ServerDomain, ChatResponseType.ListStart,
                   $"{_session.UserInfo.NickName} Channel", "");

            if (ChatChannelManager.Channels.Count != 0)
            {
               foreach(var channel in ChatChannelManager.Channels)
                {
                    //TODO
                    //add channel information here
                }
            }
            //add list response tail
            _sendingBuffer += ChatCommandBase.BuildNormalRPL(
                ChatServer.ServerDomain, ChatResponseType.ListEnd,
                _session.UserInfo.NickName, "End of /LIST");
        }
    }
}
