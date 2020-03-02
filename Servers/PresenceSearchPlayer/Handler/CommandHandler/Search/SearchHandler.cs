using GameSpyLib.Database.DatabaseModel.MySql;
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
    public class SearchHandler : GPSPHandlerBase
    {
        public SearchHandler(GPSPSession session, Dictionary<string, string> recv) : base(session, recv)
        {
        }

        private uint _profileid;
        private uint _partnerid;
        private int _skip;
        protected override void CheckRequest(GPSPSession session, Dictionary<string, string> recv)
        {
            base.CheckRequest(session, recv);
            if (!recv.ContainsKey("profileid") && !recv.ContainsKey("namespaceid") && !recv.ContainsKey("gamename") && !recv.ContainsKey("partnerid"))
            {
                _errorCode = GPErrorCode.Parse;
                return;
            }
            if (!uint.TryParse(recv["profileid"], out _profileid) || !uint.TryParse(recv["partnerid"], out _partnerid))
            {
                _errorCode = GPErrorCode.Parse;
                return;
            }
            if (recv.ContainsKey("skip"))
            {
                if (!int.TryParse(recv["skip"], out _skip))
                {
                    _errorCode = GPErrorCode.Parse;
                    return;
                }

            }

        }

        protected override void DataBaseOperation(GPSPSession session, Dictionary<string, string> recv)
        {
            //TODO verify the search condition whether needed namespaceid!!!!!
            using (var db = new retrospyContext())
            {
                //we only need uniquenick to search a profile
                if (recv.ContainsKey("uniquenick") && recv.ContainsKey("namespaceid"))
                {
                    var result = from p in db.Profiles
                                 join n in db.Subprofiles on p.Profileid equals n.Profileid
                                 join u in db.Users on p.Userid equals u.Userid
                                 where n.Uniquenick == recv["uniquenick"]
                                 && n.Namespaceid == _namespaceid
                                 && n.Gamename == recv["gamename"]
                                 && n.Partnerid == _partnerid
                                 select new
                                 {
                                     profileid = n.Profileid,
                                     nick = p.Nick,
                                     uniquenick = n.Uniquenick,
                                     email = u.Email,
                                     first = p.Firstname,
                                     last = p.Lastname
                                 };

                    foreach (var p in result.Skip(_skip))
                    {
                        _sendingBuffer = @"\bsr\" + p.profileid;
                        _sendingBuffer += @"\nick\" + p.nick;
                        _sendingBuffer += @"\uniquenick\" + p.uniquenick;
                        _sendingBuffer += @"\namespaceid\" + _namespaceid;
                        _sendingBuffer += @"\firstname\" + p.first;
                        _sendingBuffer += @"\lastname\" + p.last;
                        _sendingBuffer += @"\email\" + p.email;
                    }
                    _sendingBuffer += @"\bsrdone\\more\0\final\";



                }
                else if (recv.ContainsKey("nick") && recv.ContainsKey("email"))
                {
                    var result = from p in db.Profiles
                                 join n in db.Subprofiles on p.Profileid equals n.Profileid
                                 join u in db.Users on p.Userid equals u.Userid
                                 where p.Nick == recv["nick"] && u.Email == recv["email"]
                                 && n.Namespaceid == _namespaceid
                                 && n.Gamename == recv["gamename"]
                                 && n.Partnerid == _partnerid
                                 select new
                                 {
                                     profileid = n.Profileid,
                                     nick = p.Nick,
                                     uniquenick = n.Uniquenick,
                                     email = u.Email,
                                     first = p.Firstname,
                                     last = p.Lastname
                                 };
                    foreach (var p in result.Skip(_skip))
                    {
                        _sendingBuffer = @"\bsr\" + p.profileid;
                        _sendingBuffer += @"\nick\" + p.nick;
                        _sendingBuffer += @"\uniquenick\" + p.uniquenick;
                        _sendingBuffer += @"\namespaceid\" + _namespaceid;
                        _sendingBuffer += @"\firstname\" + p.first;
                        _sendingBuffer += @"\lastname\" + p.last;
                        _sendingBuffer += @"\email\" + p.email;
                    }
                    _sendingBuffer += @"\bsrdone\\more\0\final\";
                }
                else if (recv.ContainsKey("nick"))
                {
                    var result = from p in db.Profiles
                                 join n in db.Subprofiles on p.Profileid equals n.Profileid
                                 join u in db.Users on p.Userid equals u.Userid
                                 where p.Nick == recv["nick"]
                                 && n.Namespaceid == _namespaceid
                                 && n.Gamename == recv["gamename"]
                                 && n.Partnerid == _partnerid
                                 select new
                                 {
                                     profileid = n.Profileid,
                                     nick = p.Nick,
                                     uniquenick = n.Uniquenick,
                                     email = u.Email,
                                     first = p.Firstname,
                                     last = p.Lastname
                                 };
                    foreach (var p in result.Skip(_skip))
                    {
                        _sendingBuffer = @"\bsr\" + p.profileid;
                        _sendingBuffer += @"\nick\" + p.nick;
                        _sendingBuffer += @"\uniquenick\" + p.uniquenick;
                        _sendingBuffer += @"\namespaceid\" + _namespaceid;
                        _sendingBuffer += @"\firstname\" + p.first;
                        _sendingBuffer += @"\lastname\" + p.last;
                        _sendingBuffer += @"\email\" + p.email;
                    }
                    _sendingBuffer += @"\bsrdone\\more\0\final\";

                }
                else if (recv.ContainsKey("email"))
                {
                    var result = from p in db.Profiles
                                 join n in db.Subprofiles on p.Profileid equals n.Profileid
                                 join u in db.Users on p.Userid equals u.Userid
                                 where u.Email == recv["email"]
                                 && n.Namespaceid == _namespaceid
                                 && n.Gamename == recv["gamename"]
                                 && n.Partnerid == _partnerid
                                 select new
                                 {
                                     profileid = n.Profileid,
                                     nick = p.Nick,
                                     uniquenick = n.Uniquenick,
                                     email = u.Email,
                                     first = p.Firstname,
                                     last = p.Lastname
                                 };
                    foreach (var p in result.Skip(_skip))
                    {
                        _sendingBuffer = @"\bsr\" + p.profileid;
                        _sendingBuffer += @"\nick\" + p.nick;
                        _sendingBuffer += @"\uniquenick\" + p.uniquenick;
                        _sendingBuffer += @"\namespaceid\" + _namespaceid;
                        _sendingBuffer += @"\firstname\" + p.first;
                        _sendingBuffer += @"\lastname\" + p.last;
                        _sendingBuffer += @"\email\" + p.email;
                    }
                    _sendingBuffer += @"\bsrdone\\more\0\final\";
                }
                else
                {
                    _errorCode = GPErrorCode.DatabaseError;
                    return;
                }
            }
        }
    }
}


