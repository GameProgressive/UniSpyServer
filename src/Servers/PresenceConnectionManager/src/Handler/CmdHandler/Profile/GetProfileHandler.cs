using UniSpyServer.Servers.PresenceConnectionManager.Abstraction.BaseClass;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Contract;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure.Request.Profile;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure.Response;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure.Result;
using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Exception.General;
using System.Collections.Generic;
using System.Linq;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Database.DatabaseModel.MySql;

namespace UniSpyServer.Servers.PresenceConnectionManager.Handler.CmdHandler
{
    [HandlerContract("getprofile")]
    public sealed class GetProfileHandler : Abstraction.BaseClass.CmdHandlerBase
    {
        // \getprofile\\sesskey\19150\profileid\2\id\2\final\
        private new GetProfileRequest _request => (GetProfileRequest)base._request;

        private new GetProfileResult _result
        {
            get => (GetProfileResult)base._result;
            set => base._result = value;
        }

        public GetProfileHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _result = new GetProfileResult();
        }
        protected override void DataOperation()
        {
            using (var db = new UnispyContext())
            {
                //we have to make sure the search target has the same namespaceID
                var result = from p in db.Profiles
                             join s in db.Subprofiles on p.Profileid equals s.Profileid
                             join u in db.Users on p.Userid equals u.Userid
                             where p.Profileid == _request.ProfileID
                             && s.Namespaceid == _session.UserInfo.BasicInfo.NamespaceID
                             select new GetProfileDataModel
                             {
                                 Nick = p.Nick,
                                 ProfileID = p.Profileid,
                                 UniqueNick = s.Uniquenick,
                                 Email = u.Email,
                                 Firstname = p.Firstname,
                                 Lastname = p.Lastname,
                                 Icquin = p.Icquin,
                                 Homepage = p.Homepage,
                                 Zipcode = p.Zipcode,
                                 Countrycode = p.Countrycode,
                                 Longitude = p.Longitude,
                                 Latitude = p.Latitude,
                                 Location = p.Location,
                                 Birthday = p.Birthday,
                                 Birthmonth = p.Birthmonth,
                                 Birthyear = p.Birthyear,
                                 Sex = p.Sex,
                                 Publicmask = p.Publicmask,
                                 Aim = p.Aim,
                                 Picture = p.Picture,
                                 Occupationid = p.Occupationid,
                                 Industryid = p.Industryid,
                                 Incomeid = p.Incomeid,
                                 Marriedid = p.Marriedid,
                                 Childcount = p.Childcount,
                                 Interests1 = p.Interests1,
                                 Ownership1 = p.Ownership1,
                                 Connectiontype = p.Connectiontype,
                             };

                if (result.Count() == 0)
                {
                    throw new GPDatabaseException($"No profile of profileid:{_request.ProfileID} found in database.");
                }

                _result.UserProfile = result.First();
            }
        }

        protected override void ResponseConstruct()
        {
            _response = new GetProfileResponse(_request, _result);
        }
    }
}

