using System;
using UniSpyLib.Abstraction.Interface;

namespace PresenceConnectionManager.Handler.CommandHandler
{
    public class NewUserHandler : PresenceSearchPlayer.Handler.CommandHandler.NewUserHandler
    {
        public NewUserHandler(ISession session, IRequest request) : base(session, request)
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
