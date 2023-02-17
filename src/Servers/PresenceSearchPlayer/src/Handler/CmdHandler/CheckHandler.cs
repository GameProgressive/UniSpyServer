using UniSpy.Server.PresenceSearchPlayer.Abstraction.BaseClass;
using UniSpy.Server.PresenceSearchPlayer.Application;
using UniSpy.Server.PresenceSearchPlayer.Entity.Enumerate;
using UniSpy.Server.PresenceSearchPlayer.Entity.Structure.Exception;
using UniSpy.Server.PresenceSearchPlayer.Entity.Structure.Request;
using UniSpy.Server.PresenceSearchPlayer.Entity.Structure.Response;
using UniSpy.Server.PresenceSearchPlayer.Entity.Structure.Result;
using UniSpy.Server.Core.Abstraction.Interface;

namespace UniSpy.Server.PresenceSearchPlayer.Handler.CmdHandler
{

    public sealed class CheckHandler : CmdHandlerBase
    {
        // \check\\nick\<nick>\email\<email>\partnerid\0\passenc\<passenc>\gamename\gmtest\final\
        //\cur\pid\<pid>\final
        //check is request recieved correct and convert password into our MD5 type
        private new CheckRequest _request => (CheckRequest)base._request;
        private new CheckResult _result { get => (CheckResult)base._result; set => base._result = value; }
        public CheckHandler(IClient client, IRequest request) : base(client, request)
        {
            _result = new CheckResult();
        }

        protected override void DataOperation()
        {
            if (StorageOperation.Persistance.VerifyEmail(_request.Email))
            {
                throw new CheckException("No account exists with the provided email address.", GPErrorCode.CheckBadMail);
            }

            if (StorageOperation.Persistance.VerifyEmailAndPassword(_request.Email, _request.Password))
            {
                throw new CheckException("No account exists with the provided email address.", GPErrorCode.CheckBadPassword);
            }
            _result.ProfileId = StorageOperation.Persistance.GetProfileId(_request.Email, _request.Password, _request.Nick, (int)_request.PartnerId);
        }

        protected override void ResponseConstruct()
        {
            _response = new CheckResponse(_request, _result);
        }
    }
}
