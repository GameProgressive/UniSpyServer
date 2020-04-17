//using Chat.Entity.Structure;
//using Chat.Entity.Structure.ChatCommand;
//using Chat.Entity.Structure.Enumerator.Request;
//using Chat.Server;
//using GameSpyLib.Common.Entity.Interface;
//using System;

//namespace Chat.Handler.CommandHandler.LOGIN
//{
//    public class LOGINHandler : ChatCommandHandlerBase
//    {
//        int _namespaceID = 0;
//        string _password;
//        string _uniqueNick;

//        public LOGINHandler(IClient client, ChatCommandBase cmd, string response) : base(client, cmd, response)
//        {
//        }

//        public override void CheckRequest()
//        {
//            base.CheckRequest();

//            if (!Int32.TryParse(_cmd.Param[0], out _namespaceID))
//            {
//                _errorCode = ChatError.Parse;
//                return;
//            }
//            _clientInfo.NameSpaceID = _namespaceID;

//            if (_cmd.Param[1] == "*")
//            {
//                // Profile login, not handled yet!
//                _errorCode = ChatError.LoginFailed;
//                return;
//            }

//            // Unique nickname login
//            _uniqueNick = _cmd.Param[2];
//            _password = _cmd.Param[3];
//        }

//        public override void DataOperation()
//        {
//            base.DataOperation();
//            _clientInfo.NickName = _cmd.Param[1];
//        }

//        public override void ConstructResponse()
//        {
//            base.ConstructResponse();
//            _sendingBuffer = ChatCommandBase.BuildCommandString(ChatResponse.Login, "* 1 1");
//        }

//        public override void Response()
//        {
//            base.Response();
//            _session.SendAsync($":{_session.ClientInfo.ServerIP} 001 {_session.ClientInfo.NickName} :Welcome!\r\n");
//            _session.SendAsync(ChatServer.GenerateChatCommandBase(ChatResponse.ToPic, "#retrospy Test!"));
//            _session.SendAsync(ChatServer.GenerateChatCommandBase(ChatResponse.EndOfNames, "#retrospy :End of names LIST"));
//        }

//        public override void SetCommandName()
//        {
//            CommandName = ChatRequest.LOGIN.ToString();
//        }
//    }
//}
