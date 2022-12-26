using UniSpyServer.Servers.GameStatus.Abstraction.BaseClass;
using UniSpyServer.Servers.GameStatus.Entity.Exception;
using UniSpyServer.Servers.GameStatus.Entity.Structure.Request;
using UniSpyServer.Servers.GameStatus.Entity.Structure.Response;
using UniSpyServer.Servers.GameStatus.Entity.Structure.Result;
using System.Linq;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Database.DatabaseModel;
using UniSpyServer.Servers.GameStatus.Application;

namespace UniSpyServer.Servers.GameStatus.Handler.CmdHandler
{

    public sealed class GetProfileIdHandler : CmdHandlerBase
    {
        //request \getpid\\nick\%s\keyhash\%s\lid\%d
        //response \getpidr
        private int _profileId;
        private new GetProfileIDRequest _request => (GetProfileIDRequest)base._request;
        private new GetProfileIDResult _result { get => (GetProfileIDResult)base._result; set => base._result = value; }
        public GetProfileIdHandler(IClient client, IRequest request) : base(client, request)
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
