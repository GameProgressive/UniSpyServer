using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure.ChatCommand;
using UniSpyLib.Abstraction.Interface;

namespace Chat.Handler.CmdHandler.ChatGeneralCommandHandler
{
    public class USERHandler : ChatCmdHandlerBase
    {
        protected new USERRequest _request { get { return (USERRequest)base._request; } }

        public USERHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        protected override void DataOperation()
        {
            base.DataOperation();
            _session.UserInfo.SetUserName(_request.UserName);
            _session.UserInfo.SetName(_request.Name);
        }

        protected override void BuildNormalResponse()
        {
            base.BuildNormalResponse();

            //TODO check if this is correct
            //we postpone welcome message until when recieved NICK request
            //_sendingBuffer =
            //    ChatReply.BuildWelcomeReply(_session.UserInfo);

            _session.UserInfo.SetLoginFlag(true);


            //check this!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            //_sendingBuffer += ChatCommandBase.BuildMessageRPL(
            //_nickCmd.NickName, $"MODE {_nickCmd.NickName}", "+iwx");
        }
    }
}
