using GameSpyLib.Common;
using GameSpyLib.Database.DatabaseModel.MySql;
using PresenceSearchPlayer.Enumerator;
using System.Collections.Generic;
using System.Linq;

namespace PresenceSearchPlayer.Handler.CommandHandler.NewUser
{
    public class NewUserHandler : GPSPHandlerBase
    {
        private bool _IsUniquenickMethod;
        private string _uniquenick;
        private bool _IsEmailExist;
        private bool _IsUserPasswordCorrect;
        private bool _IsNickNameExist;
        private bool _IsUniqueNickExist;

        private uint _profileid;
        public NewUserHandler(GPSPSession session, Dictionary<string, string> recv) : base(session, recv)
        {
        }

        protected override void CheckRequest(GPSPSession session, Dictionary<string, string> recv)
        {
            base.CheckRequest(session, recv);

            if (!recv.ContainsKey("nick"))
            {
                _errorCode = GPErrorCode.Parse;
                return;
            }
            if (!recv.ContainsKey("email") || !GameSpyUtils.IsEmailFormatCorrect(recv["email"]))
            {
                _errorCode = GPErrorCode.Parse;
                return;
            }

            if (!recv.ContainsKey("passenc"))
            {
                _errorCode = GPErrorCode.Parse;
                return;
            }

            if (recv.ContainsKey("uniquenick"))
            {
                _IsUniquenickMethod = true;
                _uniquenick = recv["uniquenick"];
            }

        }

        protected override void DataBaseOperation(GPSPSession session, Dictionary<string, string> recv)
        {
            using (var db = new retrospyContext())
            {
                _IsEmailExist = db.Users.Where(u => u.Email == recv["email"]).Select(p => p.Userid).Count() == 1;

                _IsUserPasswordCorrect = db.Users.Where(u => u.Email == recv["email"] && u.Password == recv["passenc"]).Select(p => p.Userid).Count() == 1;

                _IsNickNameExist = db.Users.Join(db.Profiles, u => u.Userid, p => p.Userid, (u, p) => new { users = u, profiles = p }).
                    Where(o => o.users.Email == recv["email"]
                    && o.users.Password == recv["passenc"]
                    && o.profiles.Nick == recv["nick"]).Select(r => r.profiles.Profileid).Count() == 1;

                var result = from u in db.Users
                             join p in db.Profiles on u.Userid equals p.Userid
                             join n in db.Subprofiles on p.Profileid equals n.Profileid
                             where u.Email == recv["email"] && u.Password == recv["passenc"] && p.Nick == recv["nick"] && n.Uniquenick == recv["uniquenick"] && n.Namespaceid == _namespaceid
                             select p.Profileid;
                _IsUniqueNickExist = result.Count() == 1;


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
                    //create user in database
                    if (!_IsEmailExist)
                    {
                        Users users = new Users { Email = recv["email"], Password = recv["passenc"] };
                        db.Users.Add(users);
                        db.SaveChanges();
                        Profiles profiles = new Profiles { Userid = users.Userid, Nick = recv["nick"] };
                        db.Profiles.Add(profiles);
                        db.SaveChanges();
                        Subprofiles subprofiles = new Subprofiles { Profileid = profiles.Profileid, Namespaceid = _namespaceid, Uniquenick = _uniquenick };
                        db.Subprofiles.Add(subprofiles);
                        db.SaveChanges();
                        //set profileid for construct response
                        _profileid = profiles.Profileid;
                        //create user and profile and namespace
                        if (_IsEmailExist && _IsUserPasswordCorrect && !_IsNickNameExist)
                        {
                            uint userid = db.Users.Where(u => u.Email == recv["email"] && u.Password == recv["passenc"]).Select(u => u.Userid).First();
                            Profiles profiles1 = new Profiles { Userid = userid, Nick = recv["nick"] };
                            db.Profiles.Add(profiles1);
                            db.SaveChanges();
                            Subprofiles subprofiles1 = new Subprofiles { Profileid = profiles1.Profileid, Namespaceid = _namespaceid, Uniquenick = _uniquenick };
                            db.Subprofiles.Add(subprofiles1);
                            db.SaveChanges();
                            //set profileid for construct response
                            _profileid = profiles1.Profileid;
                            //create nick and niquenick
                        }
                        if (_IsEmailExist && _IsUserPasswordCorrect && _IsEmailExist && !_IsUniqueNickExist)
                        {
                            var resultpids = from u in db.Users
                                             join p in db.Profiles on u.Userid equals p.Userid
                                             where u.Email == recv["email"] && u.Password == recv["passenc"] && p.Nick == recv["nick"]
                                             select p.Profileid;
                            uint profileid = resultpids.First();
                            Subprofiles subprofiles2 = new Subprofiles { Profileid = profileid, Namespaceid = _namespaceid, Uniquenick = _uniquenick };
                            db.Subprofiles.Add(subprofiles2);
                            //set profileid for construct response
                            _profileid = subprofiles2.Profileid;
                            //create uniquenick
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
                    UpdateOtherInfo(session, recv);
                }

            }
        }

        protected override void ConstructResponse(GPSPSession session, Dictionary<string, string> recv)
        {
            if (_errorCode != GPErrorCode.NoError)
            {
                _sendingBuffer = string.Format(@"\nur\{0}\final\", (uint)_errorCode);
            }
            else
                _sendingBuffer = string.Format(@"\nur\0\pid\{0}\final\", _profileid);
        }

        private void UpdateOtherInfo(GPSPSession session, Dictionary<string, string> recv)
        {
            using (var db = new retrospyContext())
            {
                var resultpids = from u in db.Users
                                 join p in db.Profiles on u.Userid equals p.Userid
                                 join n in db.Subprofiles on p.Profileid equals n.Profileid
                                 where u.Email == recv["email"] && u.Password == recv["passenc"]
                                 && p.Nick == recv["nick"] && n.Uniquenick == recv["uniquenick"]
                                 && n.Namespaceid == _namespaceid
                                 select p.Profileid;
                uint profileid = resultpids.First();

                var ns = from n in db.Subprofiles
                         where n.Profileid == profileid && n.Uniquenick == recv["uniquenick"] && n.Namespaceid == _namespaceid
                         select n;
                var firstns = ns.First();
                uint partnerid;

                if (recv.ContainsKey("partnerid"))
                {
                    if (uint.TryParse(recv["partnerid"], out partnerid))
                        firstns.Partnerid = partnerid;
                    else
                        _errorCode = GPErrorCode.DatabaseError;
                }
                uint productid;
                if (recv.ContainsKey("productid"))
                    if (uint.TryParse(recv["productid"], out productid))
                    { firstns.Productid = productid; }
                    else
                    { _errorCode = GPErrorCode.DatabaseError; }

                //if (recv.ContainsKey("productID"))
                //    if (uint.TryParse(recv["productID"], out productid))
                //        firstns.Productid = productid;

                if (recv.ContainsKey("gamename"))
                {
                    firstns.Gamename = recv["gamename"];
                }
                uint port;
                if (recv.ContainsKey("port"))
                {
                    if (uint.TryParse(recv["port"], out port))
                    { firstns.Port = port; }
                    else
                    { _errorCode = GPErrorCode.DatabaseError; }

                }

                if (recv.ContainsKey("cdkeyenc"))
                {
                    firstns.Cdkeyenc = recv["cdkeyenc"];
                }
                db.Update(firstns);

            }
        }
    }
}
