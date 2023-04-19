using UniSpy.Server.GameStatus.Abstraction.BaseClass;
using UniSpy.Server.GameStatus.Contract.Request;
using UniSpy.Server.GameStatus.Contract.Response;
using UniSpy.Server.GameStatus.Contract.Result;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.GameStatus.Application;

namespace UniSpy.Server.GameStatus.Handler.CmdHandler
{

    public sealed class GetProfileIdHandler : CmdHandlerBase
    {
        //request \getpid\\nick\%s\keyhash\%s\lid\%d
        //response \getpidr
        private int _profileId;
        private new GetProfileIDRequest _request => (GetProfileIDRequest)base._request;
        private new GetProfileIDResult _result { get => (GetProfileIDResult)base._result; set => base._result = value; }
        public GetProfileIdHandler(Client client, GetProfileIDRequest request) : base(client, request)
        {
            _result = new GetProfileIDResult();
        }
        protected override void DataOperation()
        {
            _profileId = StorageOperation.Persistance.GetProfileId(_request.KeyHash, _request.Nick);
        }

        protected override void ResponseConstruct()
        {
            _response = new GetProfileIDResponse(_request, _result);
        }
    }
}
