using GameStatus.Abstraction.BaseClass;
using GameStatus.Entity.Contract;
using GameStatus.Entity.Exception;
using GameStatus.Entity.Structure.Request;
using GameStatus.Entity.Structure.Response;
using GameStatus.Entity.Structure.Result;
using System.Collections.Generic;
using System.Linq;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Database.DatabaseModel.MySql;

namespace GameStatus.Handler.CmdHandler
{
    [HandlerContract("getpid")]
    internal sealed class GetPIDHandler : CmdHandlerBase
    {
        //request \getpid\\nick\%s\keyhash\%s\lid\%d
        //response \getpidr
        private uint _protileid;
        private new GetPIDRequest _request => (GetPIDRequest)base._request;
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
