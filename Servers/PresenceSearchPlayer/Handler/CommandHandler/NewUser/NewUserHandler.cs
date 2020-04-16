using GameSpyLib.Common;
using GameSpyLib.Common.Entity.Interface;
using GameSpyLib.Database.DatabaseModel.MySql;
using GameSpyLib.MiscMethod;
using PresenceSearchPlayer.Enumerator;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PresenceSearchPlayer.Handler.CommandHandler.NewUser
{
    public class NewUserHandler : PSPCommandHandlerBase
    {
        private string _uniquenick;
        private Users _users;
        private Profiles _profiles;
        private Subprofiles _subProfiles;

        public NewUserHandler(IClient client, Dictionary<string, string> recv) : base(client, recv)
        {
        }

        protected enum _newUserStatus
        {
            CheckAccount,
            AccountNotExist,
            AccountExist,
            CheckProfile,
            ProfileNotExist,
            ProfileExist,
            CheckSubProfile,
            SubProfileNotExist,
            SubProfileExist
        }

        protected override void CheckRequest()
        {
            base.CheckRequest();

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
                _uniquenick = _recv["uniquenick"];
            }
        }

        protected override void DataOperation()
        {
            using (var db = new retrospyContext())
            {
                try
                {
                    switch (_newUserStatus.CheckAccount)
                    {
                        case _newUserStatus.CheckAccount:
                            int count = db.Users.Where(u => u.Email == _recv["email"]).Select(u => u).Count();
                            if (count == 0)
                            {
                                goto case _newUserStatus.AccountNotExist;
                            }
                            else
                            {
                                goto case _newUserStatus.AccountExist;
                            }

                        case _newUserStatus.AccountNotExist:
                            _users = new Users { Email = _recv["email"], Password = _recv["passenc"] };
                            db.Users.Add(_users);
                            db.SaveChanges();
                            goto case _newUserStatus.CheckProfile;

                        case _newUserStatus.AccountExist:
                            //we have to check password correctness
                            _users = db.Users.Where(u => u.Email == _recv["email"] && u.Password == _recv["passenc"]).FirstOrDefault();
                            if (_users == null)
                            {
                                _errorCode = GPErrorCode.NewUserBadPasswords;
                                break;
                            }
                            else
                            {
                                goto case _newUserStatus.CheckProfile;
                            }

                        case _newUserStatus.CheckProfile:
                            _profiles = db.Profiles.Where(p => p.Userid == _users.Userid && p.Nick == _recv["nick"]).FirstOrDefault();
                            if (_profiles == null)
                            {
                                goto case _newUserStatus.ProfileNotExist;
                            }
                            else
                            {
                                goto case _newUserStatus.ProfileExist;
                            }

                        case _newUserStatus.ProfileNotExist:
                            _profiles = new Profiles { Userid = _users.Userid, Nick = _recv["nick"] };
                            db.Profiles.Add(_profiles);
                            db.SaveChanges();
                            goto case _newUserStatus.CheckSubProfile;

                        case _newUserStatus.ProfileExist:
                        //we do nothing here

                        case _newUserStatus.CheckSubProfile:
                            _subProfiles = db.Subprofiles
                                .Where(s => s.Profileid == _profiles.Profileid
                                && s.Uniquenick == _uniquenick
                                && s.Namespaceid == _namespaceid).FirstOrDefault();
                            if (_subProfiles == null)
                            {
                                goto case _newUserStatus.SubProfileNotExist;
                            }
                            else
                            {
                                goto case _newUserStatus.SubProfileExist;
                            }

                        case _newUserStatus.SubProfileNotExist:
                            //we create subprofile and return
                            _subProfiles = new Subprofiles { Profileid = _profiles.Profileid, Uniquenick = _uniquenick, Namespaceid = _namespaceid };
                            db.Subprofiles.Add(_subProfiles);
                            db.SaveChanges();
                            break;

                        case _newUserStatus.SubProfileExist:
                            _errorCode = GPErrorCode.NewUserUniquenickInUse;
                            break;
                    }
                }
                catch (Exception)
                {
                    _errorCode = GPErrorCode.DatabaseError;
                }

                //update other information
                if (_errorCode != GPErrorCode.DatabaseError)
                {
                    UpdateOtherInfo(_recv);
                }
            }
        }

        protected override void ConstructResponse()
        {
            if (_errorCode != GPErrorCode.NoError)
            {
                _sendingBuffer = string.Format(@"\nur\{0}\final\", _errorCode);
            }
            else
            {
                //GPCM
                _sendingBuffer = string.Format(@"\nur\0\userid\{0}\profileid\{1}\id\1\final\", _users.Userid, _subProfiles.Profileid);
                //_sendingBuffer = string.Format(@"\nur\0\pid\{0}\final\", _subProfiles.Profileid);
            }
        }

        private void UpdateOtherInfo(Dictionary<string, string> _recv)
        {
            using (var db = new retrospyContext())
            {
                uint partnerid;

                if (_recv.ContainsKey("partnerid"))
                {
                    if (uint.TryParse(_recv["partnerid"], out partnerid))
                    {
                        _subProfiles.Partnerid = partnerid;
                    }
                    else
                    {
                        _errorCode = GPErrorCode.Parse;
                    }
                }

                uint productid;

                if (_recv.ContainsKey("productid"))
                {
                    if (uint.TryParse(_recv["productid"], out productid))
                    {
                        _subProfiles.Productid = productid;
                    }
                    else
                    {
                        _errorCode = GPErrorCode.Parse;
                    }
                }

                if (_recv.ContainsKey("gamename"))
                {
                    _subProfiles.Gamename = _recv["gamename"];
                }

                uint port;

                if (_recv.ContainsKey("port"))
                {
                    if (uint.TryParse(_recv["port"], out port))
                    {
                        _subProfiles.Port = port;
                    }
                    else
                    {
                        _errorCode = GPErrorCode.Parse;
                    }
                }

                if (_recv.ContainsKey("cdkeyenc"))
                {
                    _subProfiles.Cdkeyenc = _recv["cdkeyenc"];
                }
                db.Subprofiles.Update(_subProfiles);
                db.SaveChanges();
            }
        }
    }
}
