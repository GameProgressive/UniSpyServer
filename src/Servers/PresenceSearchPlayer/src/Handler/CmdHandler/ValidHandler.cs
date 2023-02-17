using UniSpy.Server.PresenceSearchPlayer.Abstraction.BaseClass;
using UniSpy.Server.PresenceSearchPlayer.Entity.Exception.General;
using UniSpy.Server.PresenceSearchPlayer.Entity.Structure.Request;
using UniSpy.Server.PresenceSearchPlayer.Entity.Structure.Response;
using UniSpy.Server.PresenceSearchPlayer.Entity.Structure.Result;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.PresenceSearchPlayer.Application;

namespace UniSpy.Server.PresenceSearchPlayer.Handler.CmdHandler
{

    public sealed class ValidHandler : CmdHandlerBase
    {
        private new ValidRequest _request => (ValidRequest)base._request;
        private new ValidResult _result { get => (ValidResult)base._result; set => base._result = value; }

        public ValidHandler(IClient client, IRequest request) : base(client, request)
        {
            _result = new ValidResult();
        }
        protected override void DataOperation()
        {
            try
            {
                _result.IsAccountValid = StorageOperation.Persistance.IsEmailExist(_request.Email);
            }
            catch (System.Exception e)
            {
                throw new GPDatabaseException("Unknown error occurs in database operation.", e);
            }
        }

        protected override void ResponseConstruct()
        {
            _response = new ValidResponse(_request, _result);
        }
    }
}
