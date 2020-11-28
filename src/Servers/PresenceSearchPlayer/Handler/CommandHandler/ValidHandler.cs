using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Database.DatabaseModel.MySql;
using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Structure.Request;
using System.Collections.Generic;
using System.Linq;

namespace PresenceSearchPlayer.Handler.CommandHandler
{
    public class ValidHandler : PSPCommandHandlerBase
    {
        protected new ValidRequest _request;
        private bool _isAccountValid;
        public ValidHandler(ISession session, IRequest request) : base(session, request)
        {
            _request = (ValidRequest)request;
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
                    _isAccountValid = false;
                }
                else if (result.Count() == 1)
                {
                    _isAccountValid = true;
                }

            }

        }
        protected override void BuildNormalResponse()
        {
            if (_isAccountValid)
            {
                _sendingBuffer = @"\vr\1\final\";
            }
            else
            {
                _sendingBuffer = @"\vr\0\final\";
            }
        }
    }
}
