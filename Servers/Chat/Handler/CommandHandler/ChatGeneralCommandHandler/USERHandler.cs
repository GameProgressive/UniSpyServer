using Chat.Entity.Structure;
using Chat.Entity.Structure.ChatCommand;
using Chat.Entity.Structure.ChatResponse;
using GameSpyLib.Common.Entity.Interface;

namespace Chat.Handler.CommandHandler.ChatGeneralCommandHandler
{
    public class USERHandler : ChatCommandHandlerBase
    {
        new USERRequest _request;

        public USERHandler(ISession session, ChatRequestBase request) : base(session, request)
        {
            _request = new USERRequest(request.RawRequest);
        }

        protected override void CheckRequest()
        {
            base.CheckRequest();
            if(!_request.Parse())
            {
                _errorCode = ChatError.Parse;
            }
        }

        protected override void DataOperation()
        {
            base.DataOperation();
            _session.UserInfo.SetUserName(_request.UserName);
            _session.UserInfo.SetName(_request.Name);
        }

        protected override void ConstructResponse()
        {
            base.ConstructResponse();

            _sendingBuffer =
                ChatReply.BuildWelcomeReply(_session.UserInfo);

            _session.UserInfo.SetLoginFlag(true);


            //check this!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            //_sendingBuffer += ChatCommandBase.BuildMessageRPL(
            //_nickCmd.NickName, $"MODE {_nickCmd.NickName}", "+iwx");
        }
    }
}
