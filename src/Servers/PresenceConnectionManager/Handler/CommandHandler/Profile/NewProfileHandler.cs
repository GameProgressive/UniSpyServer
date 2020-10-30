using GameSpyLib.Abstraction.Interface;
using GameSpyLib.Database.DatabaseModel.MySql;
using PresenceConnectionManager.Entity.Structure.Request.Profile;
using PresenceSearchPlayer.Entity.Enumerator;
using System.Collections.Generic;
using System.Linq;

namespace PresenceConnectionManager.Handler.CommandHandler.Profile
{
    public class NewProfileHandler : PCMCommandHandlerBase
    {
        protected NewProfileRequest _request;
        public NewProfileHandler(ISession client, Dictionary<string, string> recv) : base(client, recv)
        {
            _request = new NewProfileRequest(recv);
        }

        protected override void CheckRequest()
        {
            _errorCode = _request.Parse();
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
