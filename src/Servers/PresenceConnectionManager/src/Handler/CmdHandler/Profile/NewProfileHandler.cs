using System.Linq;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure.Request;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure.Response;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure.Result;
using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Exception.General;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Database.DatabaseModel;

namespace UniSpyServer.Servers.PresenceConnectionManager.Handler.CmdHandler.Profile
{

    public sealed class NewProfileHandler : Abstraction.BaseClass.CmdHandlerBase
    {
        private new NewProfileRequest _request => (NewProfileRequest)base._request;

        private new NewProfileResult _result { get => (NewProfileResult)base._result; set => base._result = value; }
        public NewProfileHandler(IClient client, IRequest request) : base(client, request)
        {
            _result = new NewProfileResult();
        }
        protected override void DataOperation()
        {
            using (var db = new UniSpyContext())
            {
                if (_request.IsReplaceNickName)
                {
                    var result = from p in db.Profiles
                                 where p.ProfileId == _client.Info.ProfileInfo.ProfileId
                                 && p.Nick == _request.OldNick
                                 select p;

                    if (result.Count() != 1)
                    {
                        throw new GPDatabaseException("No user infomation found in database.");
                    }
                    else
                    {
                        result.First().Nick = _request.NewNick;
                    }

                    db.Profiles.Where(p => p.ProfileId == _client.Info.ProfileInfo.ProfileId
                    && p.Nick == _request.OldNick).First().Nick = _request.NewNick;

                    db.SaveChanges();
                }
                else
                {
                    var profiles = new UniSpyLib.Database.DatabaseModel.Profile
                    {
                        ProfileId = (int)_client.Info.ProfileInfo.ProfileId,
                        Nick = _request.NewNick,
                        Userid = (int)_client.Info.UserInfo.UserId
                    };

                    db.Add(profiles);
                }
            }
            _result.ProfileId = (int)_client.Info.ProfileInfo.ProfileId;
        }

        protected override void ResponseConstruct()
        {
            _response = new NewProfileResponse(_request, _result);
        }
    }
}
