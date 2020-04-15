using Chat.Entity.Structure;
using GameSpyLib.Common.Entity.Interface;
using System;

namespace Chat.Handler.CommandHandler.LOGIN
{
    public class LOGINHandler : CommandHandlerBase
    {
        int _namespaceID = 0;

        public LOGINHandler(IClient client, string[] recv) : base(client, recv)
        {
        }

        public override void CheckRequest()
        {
            base.CheckRequest();
            if (!Int32.TryParse(_recv[1], out _namespaceID))
            {
                _errorCode = ChatError.Parse;
                return;
            }
            _session.UserInfo.NameSpaceID = _namespaceID;

            if (_recv[2] == "*")
            {
                // Profile login, not handled yet!
                _errorCode = ChatError.Login_Failed;
                return;
            }

            // Unique nickname login
            _session.UserInfo.UniqueNickName = _recv[3];
            //_session.chatUserInfo.password = _recv[4];
        }

        public override void DataOperation()
        {
            base.DataOperation();
            _session.UserInfo.NickName = _recv[1];
        }

        public override void ConstructResponse()
        {
            base.ConstructResponse();
            _sendingBuffer = ChatServer.GenerateChatCommand(ChatRPL.Login, "* 1 1");
        }

        public override void Response()
        {
            base.Response();
            _session.SendAsync($":{_session.UserInfo.ServerIP} 001 {_session.UserInfo.NickName} :Welcome!\r\n");
            _session.SendAsync(ChatServer.GenerateChatCommand(ChatRPL.ToPic, "#retrospy Test!"));
            _session.SendAsync(ChatServer.GenerateChatCommand(ChatRPL.EndOfNames, "#retrospy :End of names LIST"));
        }
    }
}
