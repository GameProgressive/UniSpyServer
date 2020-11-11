using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Database.DatabaseModel.MySql;
using PresenceConnectionManager.Entity.Structure.Request.Profile;
using PresenceSearchPlayer.Entity.Enumerate;
using System.Collections.Generic;
using System.Linq;
using PresenceConnectionManager.Abstraction.BaseClass;

namespace PresenceConnectionManager.Handler.CommandHandler
{
    public class NewProfileHandler : PCMCommandHandlerBase
    {
        protected new NewProfileRequest _request;
        public NewProfileHandler(ISession session, IRequest request) : base(session, request)
        {
            _request = (NewProfileRequest)request;
        }

        protected override void DataOperation()
        {
            using (var db = new retrospyContext())
            {
                if (_request.IsReplaceNickName)
                {
                    var result = from p in db.Profiles
                                 where p.Profileid == _session.UserData.ProfileID
                                 && p.Nick == _request.OldNick
                                 select p;

                    if (result.Count() != 1)
                    {
                        _errorCode = GPError.DatabaseError;
                    }
                    else
                    {
                        result.First().Nick = _request.NewNick;
                    }

                    db.Profiles.Where(p => p.Profileid == _session.UserData.ProfileID
                    && p.Nick == _request.OldNick).First().Nick = _request.NewNick;

                    db.SaveChanges();
                }
                else
                {
                    Profiles profiles = new Profiles
                    {
                        Profileid = _session.UserData.ProfileID,
                        Nick = _request.NewNick,
                        Userid = _session.UserData.UserID
                    };

                    db.Add(profiles);
                }
            }
        }

        protected override void BuildNormalResponse()
        {
            base.BuildNormalResponse();
            _sendingBuffer = $@"\npr\\profileid\{_session.UserData.ProfileID}\id\{_request.OperationID}\final\";
        }
    }
}
