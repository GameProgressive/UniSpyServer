using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;
using PresenceSearchPlayer.Entity.Enumerator;
using PresenceSearchPlayer.Entity.Structure.Request;
using PresenceSearchPlayer.Entity.Structure.Response;
using PresenceSearchPlayer.Entity.Structure.Result;
using Serilog.Events;
using System;
using System.Linq;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Database.DatabaseModel.MySql;
using UniSpyLib.Logging;

namespace PresenceSearchPlayer.Handler.CmdHandler
{
    public class NewUserHandler : PSPCmdHandlerBase
    {
        protected new NewUserRequest _request
        {
            get { return (NewUserRequest)base._request; }
        }

        protected new NewUserResult _result
        {
            get { return (NewUserResult)base._result; }
            set { base._result = value; }
        }

        public NewUserHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _result = new NewUserResult();
        }

        protected override void DataOperation()
        {
            using (var db = new unispyContext())
            {
                try
                {
                    DatabaseOperationByType();
                }
                catch (Exception e)
                {
                    LogWriter.ToLog(LogEventLevel.Error, e.ToString());
                    _result.ErrorCode = GPErrorCode.DatabaseError;
                }

                //update other information
                if (_result.ErrorCode == GPErrorCode.NoError)
                {
                    UpdateOtherInfo();
                }
            }
        }

        private void UpdateOtherInfo()
        {
            using (var db = new unispyContext())
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
                    _result.SubProfile.Cdkeyenc = _request.CDKeyEnc;
                }

                db.Subprofiles.Update(_result.SubProfile);
                db.SaveChanges();
            }
        }

        private void DatabaseOperationByType()
        {
            using (var db = new unispyContext())
            {
                switch (NewUserStatus.CheckAccount)
                {
                    case NewUserStatus.CheckAccount:
                        var users = db.Users.Where(u => u.Email == _request.Email)
                                                     .Select(u => u);
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
                            // double user in database
                            _result.ErrorCode = GPErrorCode.DatabaseError;
                            LogWriter.ToLog(LogEventLevel.Error, "There are two same records in User table!");
                            break;
                        }

                    case NewUserStatus.AccountNotExist:
                        _result.User = new Users { Email = _request.Email, Password = _request.Password };
                        db.Users.Add(_result.User);
                        db.SaveChanges();
                        goto case NewUserStatus.CheckProfile;

                    case NewUserStatus.AccountExist:

                        if (_result.User.Password != _request.Password)
                        {
                            _result.ErrorCode = GPErrorCode.NewUserBadPasswords;
                            break;
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
                            //same nick name can not register two profiles
                            _result.Profile = profiles.First();
                            goto case NewUserStatus.ProfileExist;
                        }
                        else
                        {
                            //there are two profiles we stop
                            _result.ErrorCode = GPErrorCode.DatabaseError;
                            LogWriter.ToLog(LogEventLevel.Error, "There are two same records in Profile table!");
                            break;
                        }

                    case NewUserStatus.ProfileNotExist:
                        _result.Profile = new Profiles { Userid = _result.User.Userid, Nick = _request.Nick };
                        db.Profiles.Add(_result.Profile);
                        db.SaveChanges();
                        goto case NewUserStatus.CheckSubProfile;

                    case NewUserStatus.ProfileExist:
                    //we do nothing here

                    case NewUserStatus.CheckSubProfile:
                        var subProfiles = db.Subprofiles
                            .Where(s => s.Profileid == _result.Profile.Profileid
                            && s.Namespaceid == _request.NamespaceID);
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
                            _result.ErrorCode = GPErrorCode.DatabaseError;
                            LogWriter.ToLog(LogEventLevel.Error, "There are two same records in SubProfile table!");
                            break;
                        }

                    case NewUserStatus.SubProfileNotExist:
                        //we create subprofile and return
                        _result.SubProfile = new Subprofiles
                        {
                            Profileid = _result.Profile.Profileid,
                            Uniquenick = _request.Uniquenick,
                            Namespaceid = _request.NamespaceID
                        };

                        db.Subprofiles.Add(_result.SubProfile);
                        db.SaveChanges();
                        break;

                    case NewUserStatus.SubProfileExist:
                        _result.ErrorCode = GPErrorCode.NewUserUniquenickInUse;
                        break;
                }
            }
        }

        protected override void ResponseConstruct()
        {
            _response = new NewUserResponse(_request, _result);
        }
    }
}
