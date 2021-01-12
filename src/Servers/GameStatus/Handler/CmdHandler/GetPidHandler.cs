using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Database.DatabaseModel.MySql;
using GameStatus.Abstraction.BaseClass;
using GameStatus.Entity.Enumerate;
using GameStatus.Entity.Structure.Request;
using System.Collections.Generic;
using System.Linq;

namespace GameStatus.Handler.CmdHandler
{
    internal sealed class GetPIDHandler : GSCmdHandlerBase
    {
        //request \getpid\\nick\%s\keyhash\%s\lid\%d
        //response \getpidr
        private uint _protileid;
        private new GetPIDRequest _request { get { return (GetPIDRequest)base._request; } }
        public GetPIDHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        protected override void DataOperation()
        {
            using (var db = new retrospyContext())
            {
                var result = from p in db.Profiles
                             join s in db.Subprofiles on p.Profileid equals s.Profileid
                             where s.Cdkeyenc == _request.KeyHash && p.Nick == _request.Nick
                             select s.Profileid;
                if (result.Count() != 1)
                {
                    _errorCode = GSErrorCode.Database;
                    return;
                }
                _protileid = result.First();
            }
        }

        protected override void ResponseConstruct()
        {
            _sendingBuffer = $@"\getpidr\{_protileid}\lid\{ _request.OperationID}";
        }
    }
}
