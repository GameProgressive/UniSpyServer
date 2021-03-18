using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Structure.Request;
using PresenceSearchPlayer.Entity.Structure.Response;
using PresenceSearchPlayer.Entity.Structure.Result;
using System.Collections.Generic;
using System.Linq;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Database.DatabaseModel.MySql;

/////////////////////////Finished?/////////////////////////////////
namespace PresenceSearchPlayer.Handler.CmdHandler
{
    /// <summary>
    /// Uses a email and namespaceid to find all nick in this account
    /// </summary>
    internal class NicksHandler : PSPCmdHandlerBase
    {
        protected new NicksResult _result
        {
            get { return (NicksResult)base._result; }
            set { base._result = value; }
        }
        protected new NicksRequest _request { get { return (NicksRequest)base._request; } }
        public NicksHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _result = new NicksResult();
        }

        protected override void DataOperation()
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

        protected override void ResponseConstruct()
        {
            _response = new NicksResponse(_request, _result);
        }
    }
}
