using UniSpyServer.PresenceSearchPlayer.Abstraction.BaseClass;
using UniSpyServer.PresenceSearchPlayer.Entity.Contract;
using UniSpyServer.PresenceSearchPlayer.Entity.Exception.General;
using UniSpyServer.PresenceSearchPlayer.Entity.Structure.Request;
using UniSpyServer.PresenceSearchPlayer.Entity.Structure.Response;
using UniSpyServer.PresenceSearchPlayer.Entity.Structure.Result;
using System.Collections.Generic;
using System.Linq;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Database.DatabaseModel.MySql;

/////////////////////////Finished?/////////////////////////////////
namespace UniSpyServer.PresenceSearchPlayer.Handler.CmdHandler
{
    /// <summary>
    /// Uses a email and namespaceid to find all nick in this account
    /// </summary>
    [HandlerContract("nicks")]
    public sealed class NicksHandler : CmdHandlerBase
    {
        private new NicksResult _result
        {
            get => (NicksResult)base._result;
            set => base._result = value;
        }
        private new NicksRequest _request => (NicksRequest)base._request;
        public NicksHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _result = new NicksResult();
        }

        protected override void DataOperation()
        {
            try
            {
                using (var db = new unispyContext())
                {
                    var result = from u in db.Users
                                 join p in db.Profiles on u.Userid equals p.Userid
                                 join n in db.Subprofiles on p.Profileid equals n.Profileid
                                 where u.Email == _request.Email
                                 && u.Password == _request.Password
                                 && n.Namespaceid == _request.NamespaceID
                                 select new NicksDataModel
                                 {
                                     NickName = p.Nick,
                                     UniqueNick = n.Uniquenick
                                 };

                    //we store data in strong type so we can use in next step
                    _result.DataBaseResults.AddRange(result.ToList());
                }
            }
            catch (System.Exception e)
            {
                throw new GPDatabaseException($"Unknown error occurs in database operation.", e);
            }
        }

        protected override void ResponseConstruct()
        {
            _response = new NicksResponse(_request, _result);
        }
    }
}
