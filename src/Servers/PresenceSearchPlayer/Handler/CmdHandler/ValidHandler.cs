using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Database.DatabaseModel.MySql;
using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Structure.Request;
using System.Collections.Generic;
using System.Linq;
using PresenceSearchPlayer.Entity.Structure.Result;

namespace PresenceSearchPlayer.Handler.CmdHandler
{
    public class ValidHandler : PSPCmdHandlerBase
    {
        protected new ValidRequest _request
        {
            get { return (ValidRequest)base._request; }
        }
        protected new ValidResult _result
        {
            get { return (ValidResult)base._result; }
            set { base._result = value; }
        }
        public ValidHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        protected override void DataOperation()
        {
            using (var db = new retrospyContext())
            {
                var result = from u in db.Users
                                 //According to FSW partnerid is not nessesary
                             where u.Email == _request.Email
                             select u.Userid;

                if (result.Count() == 0)
                {
                    _result.IsAccountValid = false;
                }
                else if (result.Count() == 1)
                {
                    _result.IsAccountValid = true;
                }

            }

        }
    }
}
