using UniSpyServer.Servers.PresenceSearchPlayer.Abstraction.BaseClass;
using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Contract;
using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Structure.Request;
using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Structure.Response;
using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Structure.Result;
using System.Linq;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Database.DatabaseModel;
//last one we search with email this may get few profile so we can not return GPErrorCode
//SearchWithEmail(client,dict );
//\search\\sesskey\0\profileid\0\namespaceid\1\partnerid\0\nick\mycrysis\uniquenick\xiaojiuwo\email\koujiangheng@live.cn\gamename\gmtest\final\
//\bsrdone\more\<more>\final\
//string sendingbuffer = 
//"\\bsr\\1\\nick\\mycrysis\\uniquenick\\1\\namespaceid\\0\\firstname\\jiangheng\\lastname\\kou\\email\\koujiangheng@live.cn\\bsrdone\\0\\final\\";
//client.Stream.SendAsync(sendingbuffer);
//\more\<number of items>\final\
//\search\sesskey\0\profileid\0\namespaceid\0\nick\gbr359_jordips\gamename\gbrome\final\

namespace UniSpyServer.Servers.PresenceSearchPlayer.Handler.CmdHandler
{
    [HandlerContract("search")]
    public sealed class SearchHandler : CmdHandlerBase
    {
        private new SearchRequest _request => (SearchRequest)base._request;

        private new SearchResult _result{ get => (SearchResult)base._result; set => base._result = value; }
        public SearchHandler(IClient client, IRequest request) : base(client, request)
        {
            _result = new SearchResult();
        }
        protected override void DataOperation()
        {
            //TODO verify the search condition whether needed namespaceid!!!!!
            using (var db = new UniSpyContext())
            {
                IQueryable<SearchDataBaseModel> result;
                switch (_request.RequestType)
                {
                    case SearchRequestType.NickSearch:
                        result = from p in db.Profiles
                                join n in db.Subprofiles on p.ProfileId equals n.ProfileId
                                join u in db.Users on p.Userid equals u.UserId
                                where p.Nick == _request.Nick
                                //&& n.Namespaceid == _request.NamespaceID
                                select new SearchDataBaseModel
                                {
                                    ProfileId = n.ProfileId,
                                    Nick = p.Nick,
                                    Uniquenick = n.Uniquenick,
                                    Email = u.Email,
                                    Firstname = p.Firstname,
                                    Lastname = p.Lastname,
                                    NamespaceID = n.NamespaceId
                                };
                        break;

                    case SearchRequestType.NickEmailSearch:
                        result = from p in db.Profiles
                                join n in db.Subprofiles on p.ProfileId equals n.ProfileId
                                join u in db.Users on p.Userid equals u.UserId
                                where p.Nick == _request.Nick && u.Email == _request.Email
                                //&& n.Namespaceid == _request.NamespaceID
                                //&& n.Gamename == _request.GameName
                                //&& n.Partnerid == _request.PartnerID
                                select new SearchDataBaseModel
                                {
                                    ProfileId = n.ProfileId,
                                    Nick = p.Nick,
                                    Uniquenick = n.Uniquenick,
                                    Email = u.Email,
                                    Firstname = p.Firstname,
                                    Lastname = p.Lastname,
                                    NamespaceID = n.NamespaceId
                                };
                        break;

                    case SearchRequestType.UniquenickNamespaceIDSearch:
                        result = from p in db.Profiles
                                join n in db.Subprofiles on p.ProfileId equals n.ProfileId
                                join u in db.Users on p.Userid equals u.UserId
                                where n.Uniquenick == _request.Uniquenick
                                && n.NamespaceId == _request.NamespaceID
                                //&& n.Gamename == _request.GameName
                                //&& n.Partnerid == _request.PartnerID
                                select new SearchDataBaseModel
                                {
                                    ProfileId = n.ProfileId,
                                    Nick = p.Nick,
                                    Uniquenick = n.Uniquenick,
                                    Email = u.Email,
                                    Firstname = p.Firstname,
                                    Lastname = p.Lastname,
                                    NamespaceID = n.NamespaceId
                                };
                        break;
                    case SearchRequestType.EmailSearch:
                        result = from p in db.Profiles
                                join n in db.Subprofiles on p.ProfileId equals n.ProfileId
                                join u in db.Users on p.Userid equals u.UserId
                                where u.Email == _request.Email
                                select new SearchDataBaseModel
                                {
                                    ProfileId = n.ProfileId,
                                    Nick = p.Nick,
                                    Uniquenick = n.Uniquenick,
                                    Email = u.Email,
                                    Firstname = p.Firstname,
                                    Lastname = p.Lastname,
                                    NamespaceID = n.NamespaceId
                                };
                        break;
                    default:
                        result = null;
                        break;
                }
                if (result == null)
                {
                    return;
                }
                _result.DataBaseResults.AddRange(result.ToList());
            }
        }

        protected override void ResponseConstruct()
        {
            _response = new SearchResponse(_request, _result);
        }
    }
}
