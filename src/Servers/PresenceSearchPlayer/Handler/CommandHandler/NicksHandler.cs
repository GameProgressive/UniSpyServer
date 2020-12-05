using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Database.DatabaseModel.MySql;
using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;
using PresenceSearchPlayer.Entity.Structure.Request;
using System.Collections.Generic;
using System.Linq;

/////////////////////////Finished?/////////////////////////////////
namespace PresenceSearchPlayer.Handler.CommandHandler
{
    internal class NickHandlerDataModel
    {
        public string Nick;
        public string Uniquenick;
    }
    /// <summary>
    /// Uses a email and namespaceid to find all nick in this account
    /// </summary>
    public class NicksHandler : PSPCommandHandlerBase
    {
        List<NickHandlerDataModel> _result;
        protected new NicksRequest _request;
        public NicksHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _request = (NicksRequest)request;
        }

        protected override void DataOperation()
        {
            try
            {
                using (var db = new retrospyContext())
                {
                    var result = from u in db.Users
                                 join p in db.Profiles on u.Userid equals p.Userid
                                 join n in db.Subprofiles on p.Profileid equals n.Profileid
                                 where u.Email == _request.Email
                                 && u.Password == _request.Password
                                 && n.Namespaceid == _request.NamespaceID
                                 select new NickHandlerDataModel { Nick = p.Nick, Uniquenick = n.Uniquenick };

                    //we store data in strong type so we can use in next step
                    _result = result.ToList();
                }
            }
            catch
            {
                _errorCode = GPErrorCode.DatabaseError;
            }
        }

        protected override void BuildNormalResponse()
        {
            _sendingBuffer = @"\nr\";
            foreach (var info in _result)
            {
                _sendingBuffer += @"\nick\";
                _sendingBuffer += info.Nick;

                if (_request.RequireUniqueNicks)
                {
                    _sendingBuffer += @"\uniquenick\";
                    _sendingBuffer += info.Uniquenick;
                }
            }
            _sendingBuffer += @"\ndone\final\";

        }
    }
}
