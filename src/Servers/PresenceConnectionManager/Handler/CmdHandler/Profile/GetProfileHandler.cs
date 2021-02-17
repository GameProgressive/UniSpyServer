using System.Collections.Generic;
using System.Linq;
using PresenceConnectionManager.Abstraction.BaseClass;
using PresenceConnectionManager.Entity.Structure.Request.Profile;
using PresenceConnectionManager.Entity.Structure.Response;
using PresenceConnectionManager.Entity.Structure.Result;
using PresenceSearchPlayer.Entity.Enumerate;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Database.DatabaseModel.MySql;

namespace PresenceConnectionManager.Handler.CmdHandler
{
    internal class GetProfileHandler : PCMCmdHandlerBase
    {
        // \getprofile\\sesskey\19150\profileid\2\id\2\final\
        protected new GetProfileRequest _request
        {
            get { return (GetProfileRequest)base._request; }
        }
        protected new GetProfileResult _result
        {
            get { return (GetProfileResult)base._result; }
            set { base._result = value; }
        }

        public GetProfileHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _result = new GetProfileResult();
        }
        protected override void DataOperation()
        {
            using (var db = new retrospyContext())
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
                    _result.ErrorCode = GPErrorCode.DatabaseError;
                    return;
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

