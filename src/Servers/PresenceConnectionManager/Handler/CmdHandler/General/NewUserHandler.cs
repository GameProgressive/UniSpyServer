using UniSpyLib.Abstraction.Interface;

namespace PresenceConnectionManager.Handler.CmdHandler
{
    public class NewUserHandler : PresenceSearchPlayer.Handler.CmdHandler.NewUserHandler
    {
        public NewUserHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        protected override void BuildNormalResponse()
        {
                //PCM NewUser
                _sendingBuffer =
                    $@"\nur\\userid\{_user.Userid}\profileid\{_subProfile.Profileid}\id\{_request.OperationID}\final\";
        }
    }
}
