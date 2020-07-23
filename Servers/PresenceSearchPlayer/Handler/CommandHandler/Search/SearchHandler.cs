using GameSpyLib.Common.Entity.Interface;
using GameSpyLib.Database.DatabaseModel.MySql;
using PresenceSearchPlayer.Entity.Structure.Request;
using PresenceSearchPlayer.Enumerator;
using System.Collections.Generic;
using System.Linq;
//last one we search with email this may get few profile so we can not return GPErrorCode
//SearchWithEmail(client,dict );
//\search\\sesskey\0\profileid\0\namespaceid\1\partnerid\0\nick\mycrysis\uniquenick\xiaojiuwo\email\koujiangheng@live.cn\gamename\gmtest\final\
//\bsrdone\more\<more>\final\
//string sendingbuffer = 
//"\\bsr\\1\\nick\\mycrysis\\uniquenick\\1\\namespaceid\\0\\firstname\\jiangheng\\lastname\\kou\\email\\koujiangheng@live.cn\\bsrdone\\0\\final\\";
//client.Stream.SendAsync(sendingbuffer);
//\more\<number of items>\final\
//\search\sesskey\0\profileid\0\namespaceid\0\nick\gbr359_jordips\gamename\gbrome\final\

namespace PresenceSearchPlayer.Handler.CommandHandler.Search
{
    internal class SearchDBResult
    {
       public uint Profileid;
        public string Nick;
        public string Uniquenick;
        public string Email;
        public string Firstname;
        public string Lastname;
        public uint NamespaceID;
    }

    public class SearchHandler : PSPCommandHandlerBase
    {
        protected new SearchRequest _request;
        private List<SearchDBResult> _result;
        public SearchHandler(ISession client, Dictionary<string, string> recv) : base(client, recv)
        {
            _request = new SearchRequest(recv);
            _result = new List<SearchDBResult>();
        }

        protected override void CheckRequest()
        {
            _errorCode = _request.Parse();
        }


        protected override void DataOperation()
        {
            //TODO verify the search condition whether needed namespaceid!!!!!
            using (var db = new retrospyContext())
            {
                switch (_request.RequestType)
                {
                    case SearchRequestType.NickSearch:
                        var result = from p in db.Profiles
                                     join n in db.Subprofiles on p.Profileid equals n.Profileid
                                     join u in db.Users on p.Userid equals u.Userid
                                     where p.Nick == _request.Nick
                                     && n.Namespaceid == _request.NamespaceID
                                     && n.Gamename == _request.GameName
                                     && n.Partnerid == _request.PartnerID
                                     select new SearchDBResult
                                     {
                                         Profileid = n.Profileid,
                                         Nick = p.Nick,
                                         Uniquenick = n.Uniquenick,
                                         Email = u.Email,
                                         Firstname = p.Firstname,
                                         Lastname = p.Lastname,
                                         NamespaceID = n.Namespaceid
                                     };
                        _result.AddRange(result.ToList().Skip(_request.SkipNumber));
                        break;
                    case SearchRequestType.NickEmailSearch:
                        var result2 = from p in db.Profiles
                                     join n in db.Subprofiles on p.Profileid equals n.Profileid
                                     join u in db.Users on p.Userid equals u.Userid
                                     where p.Nick == _request.Nick && u.Email ==_request.Email
                                     //&& n.Namespaceid == _request.NamespaceID
                                     && n.Gamename == _request.GameName
                                     && n.Partnerid == _request.PartnerID
                                     select new SearchDBResult
                                     {
                                         Profileid = n.Profileid,
                                         Nick = p.Nick,
                                         Uniquenick = n.Uniquenick,
                                         Email = u.Email,
                                         Firstname = p.Firstname,
                                         Lastname = p.Lastname,
                                         NamespaceID = n.Namespaceid
                                     };
                        _result.AddRange(result2.ToList().Skip(_request.SkipNumber));
                        break;

                    case SearchRequestType.UniquenickNamespaceIDSearch:
                        var result3 = from p in db.Profiles
                                     join n in db.Subprofiles on p.Profileid equals n.Profileid
                                     join u in db.Users on p.Userid equals u.Userid
                                     where n.Uniquenick == _request.Uniquenick
                                     && n.Namespaceid == _request.NamespaceID
                                     && n.Gamename == _request.GameName
                                     && n.Partnerid == _request.PartnerID
                                     select new SearchDBResult
                                     {
                                         Profileid = n.Profileid,
                                         Nick = p.Nick,
                                         Uniquenick = n.Uniquenick,
                                         Email = u.Email,
                                         Firstname = p.Firstname,
                                         Lastname = p.Lastname,
                                         NamespaceID = n.Namespaceid
                                     };

                        _result.AddRange(result3.ToList().Skip(_request.SkipNumber));
                        break;
                }
            }
        }

        protected override void ConstructResponse()
        {
            _sendingBuffer = @"\bsr\";
            foreach (var info in _result.Skip(_request.SkipNumber))
            {
                _sendingBuffer += info.Profileid;
                _sendingBuffer += @"\nick\" +info.Nick;
                _sendingBuffer += @"\uniquenick\" + info.Uniquenick;
                _sendingBuffer += @"\namespaceid\" + info.NamespaceID;
                _sendingBuffer += @"\firstname\" + info.Firstname;
                _sendingBuffer += @"\lastname\" + info.Lastname;
                _sendingBuffer += @"\email\" + info.Email;
            }

            _sendingBuffer += @"\bsrdone\\more\0";
            base.ConstructResponse();
        }

    }
}
