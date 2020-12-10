using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Database.DatabaseModel.MySql;
using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Structure.Request;
using System.Collections.Generic;
using System.Linq;

namespace PresenceSearchPlayer.Handler.CmdHandler
{
    internal class OthersListDBResult
    {
        public uint ProfileID;
        public string Uniquenick;
    }
    public class OthersListHandler : PSPCommandHandlerBase
    {
        protected new OthersListRequest _request { get { return (OthersListRequest)base._request; } }
        private List<OthersListDBResult> _result;

        public OthersListHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _result = new List<OthersListDBResult>();
        }

        //request: \otherslist\sesskey\<searcher's sesskey>\profileid\<searcher's pid>\numopids\<how many pid in his list>
        //\opids\|<opid1>|<opid2>|******\namespaceid\<>\gamename\<>\final\

        protected override void DataOperation()
        {
            using (var db = new retrospyContext())
            {
                _sendingBuffer = @"\otherslist\";

                foreach (var pid in _request.ProfileIDs)
                {
                    var result = from n in db.Subprofiles
                                 where n.Profileid == pid && n.Namespaceid == _request.NamespaceID
                                 //select new { uniquenick = n.Uniquenick };
                                 select new OthersListDBResult { ProfileID = n.Profileid, Uniquenick = n.Uniquenick };

                    _result.AddRange(result.ToList());
                }
            }
        }

        protected override void BuildNormalResponse()
        {
            _sendingBuffer = @"\otherslist\";
            foreach (var info in _result)
            {
                _sendingBuffer += $@"\o\{info.ProfileID}";
                _sendingBuffer += $@"\uniquenick\{info.Uniquenick}";
            }

            _sendingBuffer += @"oldone";

        }
    }
}
