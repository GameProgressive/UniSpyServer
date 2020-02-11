using GameSpyLib.Common;
using GameSpyLib.Database.DatabaseModel.MySql;
using PresenceSearchPlayer.Enumerator;
using System.Collections.Generic;
using System.Linq;
using LinqToDB;
using System;

namespace PresenceSearchPlayer.Handler.CommandHandler.NewUser
{
    public class NewUserHandler : GPSPHandlerBase
    {
        private bool _IsUniquenickMethod;

        bool _IsEmailExist;
        bool _IsUserPasswordCorrect;
        bool _IsNickNameExist;
        bool _IsUniqueNickExist;

        private uint _profileid;

        public NewUserHandler(Dictionary<string, string> recv) : base(recv)
        {
        }

        protected override void CheckRequest(GPSPSession session)
        {
            base.CheckRequest(session);

            if (!_recv.ContainsKey("nick"))
            {
                _errorCode = GPErrorCode.Parse;
                return;
            }
            if (!_recv.ContainsKey("email") || !GameSpyUtils.IsEmailFormatCorrect(_recv["email"]))
            {
                _errorCode = GPErrorCode.Parse;
                return;
            }

            if (!_recv.ContainsKey("passenc"))
            {
                _errorCode = GPErrorCode.Parse;
                return;
            }

            if (_recv.ContainsKey("uniquenick"))
            {
                _IsUniquenickMethod = true;
            }
        }

        protected override void DataBaseOperation(GPSPSession session)
        {
            using (var db = new RetrospyDB())
            {
                _IsEmailExist = db.Users.Where(u => u.Email == _recv["email"]).Count() == 0;

                _IsUserPasswordCorrect = db.Users.Where(u => u.Email == _recv["email"] && u.Password == _recv["passenc"]).Count() == 0;

                _IsNickNameExist = db.Users.Join(db.Profiles, u => u.Userid, p => p.Userid, (u, p) => new { users = u, profiles = p }).
                    Where(o => o.users.Email == _recv["email"]
                    && o.users.Password == _recv["passenc"]
                    && o.profiles.Nick == _recv["nick"]).Count() == 0;

                var result = from u in db.Users
                             join p in db.Profiles on u.Userid equals p.Userid
                             join n in db.Namespaces on p.Profileid equals n.Profileid
                             where u.Email == _recv["email"] && u.Password == _recv["passenc"] && p.Nick == _recv["nick"] && n.Uniquenick == _recv["uniquenick"] && n.Namespaceid == _namespaceid
                             select p.Profileid;
                _IsUniqueNickExist = result.Count() == 0;


                if (_IsEmailExist && !_IsUserPasswordCorrect)
                {
                    _errorCode = GPErrorCode.NewUserBadPasswords;
                    return;
                }

                if (_IsEmailExist && _IsUserPasswordCorrect && _IsNickNameExist && !_IsUniquenickMethod)
                {
                    _errorCode = GPErrorCode.NewUserBadNick;
                    return;
                }
                if (_IsEmailExist && _IsUserPasswordCorrect && _IsNickNameExist && _IsUniqueNickExist && _IsUniquenickMethod)
                {
                    _errorCode = GPErrorCode.NewUserUniquenickInUse;
                    return;
                }

                try
                {
                    using (db.BeginTransaction())
                    {
                        //create user in database
                        if (!_IsEmailExist)
                        {
                            db.GetTable<User>().Insert(() => new User { Email = _recv["email"], Password = _recv["passenc"] });
                            uint userid = db.Users.Where(u => u.Email == _recv["email"] && u.Password == _recv["passenc"]).Select(u => u.Userid).First();
                            db.GetTable<Profile>().Insert(() => new Profile { Userid = userid, Nick = _recv["nick"] });
                            uint profileid = db.Profiles.Where(p => p.Userid == userid && p.Nick == _recv["nick"]).Select(p => p.Profileid).First();
                            db.GetTable<@namespace>().Insert(() => new @namespace { Profileid = profileid, Namespaceid = _namespaceid });
                            //set profileid for construct response
                            _profileid = profileid;
                            //create user and profile and namespace
                            return;
                        }

                        if (_IsEmailExist && _IsUserPasswordCorrect && !_IsNickNameExist)
                        {
                            uint userid = db.Users.Where(u => u.Email == _recv["email"] && u.Password == _recv["passenc"]).Select(u => u.Userid).First();
                            db.GetTable<Profile>().Insert(() => new Profile { Userid = userid, Nick = _recv["nick"] });
                            uint profileid = db.Profiles.Where(p => p.Userid == userid && p.Nick == _recv["nick"]).Select(p => p.Profileid).First();
                            db.GetTable<@namespace>().Insert(() => new @namespace { Profileid = profileid, Namespaceid = _namespaceid });
                            //set profileid for construct response
                            _profileid = profileid;
                            //create nick and niquenick
                            return;
                        }
                        if (_IsEmailExist && _IsUserPasswordCorrect && _IsEmailExist && !_IsUniqueNickExist)
                        {
                            var resultpids = from u in db.Users
                                             join p in db.Profiles on u.Userid equals p.Userid
                                             join n in db.Namespaces on p.Profileid equals n.Profileid
                                             where u.Email == _recv["email"] && u.Password == _recv["passenc"] && p.Nick == _recv["nick"] && n.Uniquenick == _recv["uniquenick"] && n.Namespaceid == _namespaceid
                                             select p.Profileid;
                            uint profileid = resultpids.First();
                            db.GetTable<@namespace>().Insert(() => new @namespace { Profileid = profileid, Namespaceid = _namespaceid });
                            //set profileid for construct response
                            _profileid = profileid;
                            //create uniquenick
                            return;
                        }
                    }
                }
                catch
                {
                    _errorCode = GPErrorCode.DatabaseError;
                }

                //update other information
                if (_errorCode != GPErrorCode.DatabaseError)
                {
                    using (db.BeginTransaction())
                    {
                        var resultpids = from u in db.Users
                                         join p in db.Profiles on u.Userid equals p.Userid
                                         join n in db.Namespaces on p.Profileid equals n.Profileid
                                         where u.Email == _recv["email"] && u.Password == _recv["passenc"]
                                         && p.Nick == _recv["nick"] && n.Uniquenick == _recv["uniquenick"]
                                         && n.Namespaceid == _namespaceid
                                         select p.Profileid;
                        uint profileid = resultpids.First();

                        var ns = from n in db.Namespaces
                                 where n.Profileid == profileid && n.Uniquenick == _recv["uniquenick"] && n.Namespaceid == _namespaceid
                                 select n;
                        var firstns = ns.First();
                        uint partnerid;

                        if (_recv.ContainsKey("partnerid") && uint.TryParse(_recv["partnerid"], out partnerid))
                        {
                            firstns.Partnerid = partnerid;
                        }
                        uint productid;
                        if (_recv.ContainsKey("productid") && uint.TryParse(_recv["productid"], out productid))
                        {
                            firstns.Productid = productid;
                        }

                        if (_recv.ContainsKey("gamename"))
                        {
                            firstns.Gamename = _recv["gamename"];
                        }
                        uint port;
                        if (_recv.ContainsKey("port") && uint.TryParse(_recv["port"], out port))
                        {
                            firstns.Port = port;
                        }
                        if (_recv.ContainsKey("cdkeyenc"))
                        {
                            firstns.Cdkeyenc = _recv["cdkeyenc"];
                        }
                        db.Update(firstns);
                    }

                }

            }
        }

        protected override void ConstructResponse(GPSPSession session)
        {
            if (_errorCode != GPErrorCode.NoError)
            {
                _sendingBuffer = string.Format(@"\nur\{0}\final\", (uint)_errorCode);
            }
            else
                _sendingBuffer = string.Format(@"\nur\0\pid\{0}\final\", _profileid);
        }
    }
}
