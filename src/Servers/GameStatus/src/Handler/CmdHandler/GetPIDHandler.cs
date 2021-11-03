using UniSpyServer.Servers.GameStatus.Abstraction.BaseClass;
using UniSpyServer.Servers.GameStatus.Entity.Contract;
using UniSpyServer.Servers.GameStatus.Entity.Exception;
using UniSpyServer.Servers.GameStatus.Entity.Structure.Request;
using UniSpyServer.Servers.GameStatus.Entity.Structure.Response;
using UniSpyServer.Servers.GameStatus.Entity.Structure.Result;
using System.Collections.Generic;
using System.Linq;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Database.DatabaseModel.MySql;

namespace UniSpyServer.Servers.GameStatus.Handler.CmdHandler
{
    [HandlerContract("getpid")]
    public sealed class GetPIDHandler : CmdHandlerBase
    {
        //request \getpid\\nick\%s\keyhash\%s\lid\%d
        //response \getpidr
        private uint _protileid;
        private new GetProfileIDRequest _request => (GetProfileIDRequest)base._request;
        private new GetPIDResult _result
        {
            get => (GetPIDResult)base._result;
            set => base._result = value;
        }
        public GetPIDHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _result = new GetPIDResult();
        }
        protected override void DataOperation()
        {
            using (var db = new unispyContext())
            {
                var result = from p in db.Profiles
                             join s in db.Subprofiles on p.Profileid equals s.Profileid
                             where s.Cdkeyenc == _request.KeyHash && p.Nick == _request.Nick
                             select s.Profileid;
                if (result.Count() != 1)
                {
                    throw new GSException("No records found in database by authtoken.");
                }
                _protileid = result.First();
            }
        }

        protected override void ResponseConstruct()
        {
            _response = new GetPIDResponse(_request, _result);
        }
    }
}
