using UniSpyServer.Servers.PresenceSearchPlayer.Abstraction.BaseClass;
using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Contract;
using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Enumerator;
using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Exception.General;
using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Exception.NewUser;
using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Structure.Request;
using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Structure.Response;
using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Structure.Result;
using System;
using System.Linq;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Database.DatabaseModel;

namespace UniSpyServer.Servers.PresenceSearchPlayer.Handler.CmdHandler
{
    [HandlerContract("newuser")]
    public sealed class NewUserHandler : CmdHandlerBase
    {
        private new NewUserRequest _request => (NewUserRequest)base._request;

        private new NewUserResult _result{ get => (NewUserResult)base._result; set => base._result = value; }

        public NewUserHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _result = new NewUserResult();
        }

        protected override void DataOperation()
        {
            using (var db = new UniSpyContext())
            {
                try
                {
                    DatabaseOperationByType();
                }
                catch (Exception e)
                {
                    throw new GPDatabaseException("Unknown error occurs in database operation.", e);
                }
                UpdateOtherInfo();
            }
        }

        private void UpdateOtherInfo()
        {
            using (var db = new UniSpyContext())
            {

                if (_request.HasPartnerIDFlag)
                {
                    _result.SubProfile.Partnerid = _request.PartnerID;
                }

                if (_request.HasProductIDFlag)
                {
                    _result.SubProfile.Productid = _request.ProductID;
                }

                if (_request.HasGameNameFlag)
                {
                    _result.SubProfile.Gamename = _request.GameName;
                }

                if (_request.HasGamePortFlag)
                {
                    _result.SubProfile.Port = _request.GamePort;
                }

                if (_request.HasCDKeyEncFlag)
                {
                    _result.SubProfile.Cdkeyenc = _request.CDKey;
                }
                db.Subprofiles.Update(_result.SubProfile);
                db.SaveChanges();
            }
        }

        private void DatabaseOperationByType()
        {
            using (var db = new UniSpyContext())
            {
                switch (NewUserStatus.CheckAccount)
                {
                    case NewUserStatus.CheckAccount:
                        var users = db.Users.Where(u => u.Email == _request.Email).Select(u => u);
                        if (users.Count() == 0)
                        {
                            goto case NewUserStatus.AccountNotExist;
                        }
                        else if (users.Count() == 1)
                        {
                            _result.User = users.First();
                            goto case NewUserStatus.AccountExist;
                        }
                        else
                        {
                            throw new GPNewUserBadNickException("There are two same records in User table!");
                        }

                    case NewUserStatus.AccountNotExist:
                        _result.User = new User { Email = _request.Email, Password = _request.Password };
                        db.Users.Add(_result.User);
                        db.SaveChanges();
                        goto case NewUserStatus.CheckProfile;

                    case NewUserStatus.AccountExist:

                        if (_result.User.Password != _request.Password)
                        {
                            throw new GPNewUserBadPasswordException("password is incorrect when creating new user.");
                        }
                        else
                        {
                            goto case NewUserStatus.CheckProfile;
                        }

                    case NewUserStatus.CheckProfile:
                        var profiles = db.Profiles.Where(p => p.Userid == _result.User.Userid && p.Nick == _request.Nick);
                        if (profiles.Count() == 0)
                        {
                            goto case NewUserStatus.ProfileNotExist;
                        }
                        else if (profiles.Count() == 1)
                        {
                            _result.Profile = profiles.First();
                            goto case NewUserStatus.ProfileExist;
                        }
                        else
                        {
                            throw new GPNewUserUniquenickInUseException("There are two same records in Profile table.");
                        }

                    case NewUserStatus.ProfileNotExist:
                        _result.Profile = new Profile { Userid = _result.User.Userid, Nick = _request.Nick };
                        db.Profiles.Add(_result.Profile);
                        db.SaveChanges();
                        goto case NewUserStatus.CheckSubProfile;

                    case NewUserStatus.ProfileExist:
                    //we do nothing here

                    case NewUserStatus.CheckSubProfile:
                        var subProfiles = db.Subprofiles.Where(s => s.ProfileId == _result.Profile.ProfileId && s.Namespaceid == _request.NamespaceID);
                        if (subProfiles.Count() == 0)
                        {
                            goto case NewUserStatus.SubProfileNotExist;
                        }
                        else if (subProfiles.Count() == 1)
                        {
                            _result.SubProfile = subProfiles.First();
                            goto case NewUserStatus.SubProfileExist;
                        }
                        else
                        {
                            throw new GPNewUserUniquenickInUseException("There are two same records in SubProfile table!");
                        }

                    case NewUserStatus.SubProfileNotExist:
                        //we create subprofile and return
                        _result.SubProfile = new Subprofile
                        {
                            ProfileId = _result.Profile.ProfileId,
                            Uniquenick = _request.Uniquenick,
                            Namespaceid = _request.NamespaceID
                        };
                        db.Subprofiles.Add(_result.SubProfile);
                        db.SaveChanges();
                        break;

                    case NewUserStatus.SubProfileExist:
                        throw new GPNewUserUniquenickInUseException("unique nick is in use.");
                }
            }
        }

        protected override void ResponseConstruct()
        {
            _response = new NewUserResponse(_request, _result);
        }
    }
}
