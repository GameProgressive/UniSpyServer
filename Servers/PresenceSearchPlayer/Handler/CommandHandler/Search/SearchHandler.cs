using GameSpyLib.Common.Entity.Interface;
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
    public class SearchHandler : PSPCommandHandlerBase
    {
        private uint _profileid;
        private uint _partnerid;
        private int _skip;

        public SearchHandler(ISession client, Dictionary<string, string> recv) : base(client, recv)
        {
        }

        protected override void CheckRequest()
        {
            base.CheckRequest();

            if (!_recv.ContainsKey("profileid") && !_recv.ContainsKey("namespaceid") && !_recv.ContainsKey("gamename") && !_recv.ContainsKey("partnerid"))
            {
                _errorCode = GPErrorCode.Parse;
                return;
            }

            if (!uint.TryParse(_recv["profileid"], out _profileid) || !uint.TryParse(_recv["partnerid"], out _partnerid))
            {
                _errorCode = GPErrorCode.Parse;
                return;
            }

            if (_recv.ContainsKey("skip"))
            {
                if (!int.TryParse(_recv["skip"], out _skip))
                {
                    _errorCode = GPErrorCode.Parse;
                    return;
                }
            }
        }

        protected override void DataOperation()
        {
            //TODO verify the search condition whether needed namespaceid!!!!!
            using (var db = new retrospyContext())
            {
                //we only need uniquenick to search a profile
                if (_recv.ContainsKey("uniquenick") && _recv.ContainsKey("namespaceid"))
                {
                    var result = from p in db.Profiles
                                 join n in db.Subprofiles on p.Profileid equals n.Profileid
                                 join u in db.Users on p.Userid equals u.Userid
                                 where n.Uniquenick == _recv["uniquenick"]
                                 && n.Namespaceid == _namespaceid
                                 && n.Gamename == _recv["gamename"]
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
                else if (_recv.ContainsKey("nick") && _recv.ContainsKey("email"))
                {
                    var result = from p in db.Profiles
                                 join n in db.Subprofiles on p.Profileid equals n.Profileid
                                 join u in db.Users on p.Userid equals u.Userid
                                 where p.Nick == _recv["nick"] && u.Email == _recv["email"]
                                 && n.Namespaceid == _namespaceid
                                 && n.Gamename == _recv["gamename"]
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
                else if (_recv.ContainsKey("nick"))
                {
                    var result = from p in db.Profiles
                                 join n in db.Subprofiles on p.Profileid equals n.Profileid
                                 join u in db.Users on p.Userid equals u.Userid
                                 where p.Nick == _recv["nick"]
                                 && n.Namespaceid == _namespaceid
                                 && n.Gamename == _recv["gamename"]
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
                else if (_recv.ContainsKey("email"))
                {
                    var result = from p in db.Profiles
                                 join n in db.Subprofiles on p.Profileid equals n.Profileid
                                 join u in db.Users on p.Userid equals u.Userid
                                 where u.Email == _recv["email"]
                                 && n.Namespaceid == _namespaceid
                                 && n.Gamename == _recv["gamename"]
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
