using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Contract;
using PresenceSearchPlayer.Entity.Structure.Request;
using PresenceSearchPlayer.Entity.Structure.Response;
using PresenceSearchPlayer.Entity.Structure.Result;
using System.Collections.Generic;
using System.Linq;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Database.DatabaseModel.MySql;
//last one we search with email this may get few profile so we can not return GPErrorCode
//SearchWithEmail(client,dict );
//\search\\sesskey\0\profileid\0\namespaceid\1\partnerid\0\nick\mycrysis\uniquenick\xiaojiuwo\email\koujiangheng@live.cn\gamename\gmtest\final\
//\bsrdone\more\<more>\final\
//string sendingbuffer = 
//"\\bsr\\1\\nick\\mycrysis\\uniquenick\\1\\namespaceid\\0\\firstname\\jiangheng\\lastname\\kou\\email\\koujiangheng@live.cn\\bsrdone\\0\\final\\";
//client.Stream.SendAsync(sendingbuffer);
//\more\<number of items>\final\
//\search\sesskey\0\profileid\0\namespaceid\0\nick\gbr359_jordips\gamename\gbrome\final\

namespace PresenceSearchPlayer.Handler.CmdHandler
{
    [HandlerContract("search")]
    internal sealed class SearchHandler : CmdHandlerBase
    {
        private new SearchRequest _request => (SearchRequest)base._request;

        private new SearchResult _result
        {
            get => (SearchResult)base._result;
            set => base._result = value;
        }
        public SearchHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _result = new SearchResult();
        }
        protected override void DataOperation()
        {
            //TODO verify the search condition whether needed namespaceid!!!!!
            using (var db = new unispyContext())
            {
                IQueryable<SearchDataBaseModel> result;
                switch (_request.RequestType)
                {
                    case SearchRequestType.NickSearch:
                        result = from p in db.Profiles
                                 join n in db.Subprofiles on p.Profileid equals n.Profileid
                                 join u in db.Users on p.Userid equals u.Userid
                                 where p.Nick == _request.Nick
                                 //&& n.Namespaceid == _request.NamespaceID
                                 select new SearchDataBaseModel
                                 {
                                     Profileid = n.Profileid,
                                     Nick = p.Nick,
                                     Uniquenick = n.Uniquenick,
                                     Email = u.Email,
                                     Firstname = p.Firstname,
                                     Lastname = p.Lastname,
                                     NamespaceID = n.Namespaceid
                                 };
                        break;

                    case SearchRequestType.NickEmailSearch:
                        result = from p in db.Profiles
                                 join n in db.Subprofiles on p.Profileid equals n.Profileid
                                 join u in db.Users on p.Userid equals u.Userid
                                 where p.Nick == _request.Nick && u.Email == _request.Email
                                 //&& n.Namespaceid == _request.NamespaceID
                                 //&& n.Gamename == _request.GameName
                                 //&& n.Partnerid == _request.PartnerID
                                 select new SearchDataBaseModel
                                 {
                                     Profileid = n.Profileid,
                                     Nick = p.Nick,
                                     Uniquenick = n.Uniquenick,
                                     Email = u.Email,
                                     Firstname = p.Firstname,
                                     Lastname = p.Lastname,
                                     NamespaceID = n.Namespaceid
                                 };
                        break;

                    case SearchRequestType.UniquenickNamespaceIDSearch:
                        result = from p in db.Profiles
                                 join n in db.Subprofiles on p.Profileid equals n.Profileid
                                 join u in db.Users on p.Userid equals u.Userid
                                 where n.Uniquenick == _request.Uniquenick
                                 && n.Namespaceid == _request.NamespaceID
                                 //&& n.Gamename == _request.GameName
                                 //&& n.Partnerid == _request.PartnerID
                                 select new SearchDataBaseModel
                                 {
                                     Profileid = n.Profileid,
                                     Nick = p.Nick,
                                     Uniquenick = n.Uniquenick,
                                     Email = u.Email,
                                     Firstname = p.Firstname,
                                     Lastname = p.Lastname,
                                     NamespaceID = n.Namespaceid
                                 };
                        break;
                    case SearchRequestType.EmailSearch:
                        result = from p in db.Profiles
                                 join n in db.Subprofiles on p.Profileid equals n.Profileid
                                 join u in db.Users on p.Userid equals u.Userid
                                 where u.Email == _request.Email
                                 select new SearchDataBaseModel
                                 {
                                     Profileid = n.Profileid,
                                     Nick = p.Nick,
                                     Uniquenick = n.Uniquenick,
                                     Email = u.Email,
                                     Firstname = p.Firstname,
                                     Lastname = p.Lastname,
                                     NamespaceID = n.Namespaceid
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
