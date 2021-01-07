using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure.ChatCommand;
using Chat.Handler.SystemHandler.ChatSessionManage;
using UniSpyLib.Abstraction.Interface;

namespace Chat.Handler.CmdHandler.General
{
    public class QUITHandler : ChatLogedInHandlerBase
    {
        new QUITRequest _request { get { return (QUITRequest)base._request; } }
        public QUITHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        protected override void DataOperation()
        {
            base.DataOperation();

            foreach (var channel in _session.UserInfo.JoinedChannels)
            {
                channel.LeaveChannel(_session, _request.Reason);
            }
            ChatSessionManager.RemoveSession(_session);
        }
    }
}
