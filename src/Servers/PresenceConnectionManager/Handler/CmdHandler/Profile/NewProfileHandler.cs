using PresenceConnectionManager.Abstraction.BaseClass;
using PresenceConnectionManager.Entity.Structure.Request.Profile;
using PresenceConnectionManager.Entity.Structure.Response;
using PresenceConnectionManager.Entity.Structure.Result;
using PresenceSearchPlayer.Entity.Enumerate;
using System.Linq;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Database.DatabaseModel.MySql;

namespace PresenceConnectionManager.Handler.CmdHandler
{
    internal class NewProfileHandler : PCMCmdHandlerBase
    {
        protected new NewProfileRequest _request
        {
            get => (NewProfileRequest)base._request;
        }
        protected new NewProfileResult _result
        {
            get => (NewProfileResult)base._result;
            set => base._result = value;
        }
        public NewProfileHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _result = new NewProfileResult();
        }
        protected override void DataOperation()
        {
            using (var db = new unispyContext())
            {
                if (_request.IsReplaceNickName)
                {
                    var result = from p in db.Profiles
                                 where p.Profileid == _session.UserInfo.BasicInfo.ProfileID
                                 && p.Nick == _request.OldNick
                                 select p;

                    if (result.Count() != 1)
                    {
                        _result.ErrorCode = GPErrorCode.DatabaseError;
                    }
                    else
                    {
                        result.First().Nick = _request.NewNick;
                    }

                    db.Profiles.Where(p => p.Profileid == _session.UserInfo.BasicInfo.ProfileID
                    && p.Nick == _request.OldNick).First().Nick = _request.NewNick;

                    db.SaveChanges();
                }
                else
                {
                    Profiles profiles = new Profiles
                    {
                        Profileid = _session.UserInfo.BasicInfo.ProfileID,
                        Nick = _request.NewNick,
                        Userid = _session.UserInfo.BasicInfo.UserID
                    };

                    db.Add(profiles);
                }
            }
            _result.ProfileID = _session.UserInfo.BasicInfo.ProfileID;
        }

        protected override void ResponseConstruct()
        {
            _response = new NewProfileResponse(_request, _result);
        }
    }
}
