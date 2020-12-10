using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Database.DatabaseModel.MySql;
using UniSpyLib.MiscMethod;
using PresenceConnectionManager.Entity.Structure.Request.Profile;
using PresenceSearchPlayer.Entity.Enumerate;
using System.Collections.Generic;
using System.Linq;
using PresenceConnectionManager.Abstraction.BaseClass;

namespace PresenceConnectionManager.Handler.CmdHandler
{
    public class GetProfileDBResult
    {
        public string Nick;
        public uint ProfileID;
        public string UniqueNick;
        public string Email;
        public string Firstname;
        public string Lastname;
        public uint? Icquin;
        public string Homepage;
        public string Zipcode;
        public string Countrycode;
        public float Longitude;
        public float Latitude;
        public string Location;
        public int? Birthday;
        public int? Birthmonth;
        public int? Birthyear;
        public byte? Sex;
        public int Publicmask;
        public string Aim;
        public int Picture;
        public int? Occupationid;
        public int? Industryid;
        public int? Incomeid;
        public int? Marriedid;
        public int? Childcount;
        public int? Interests1;
        public int? Ownership1;
        public int? Connectiontype;
    }

    public class GetProfileHandler : PCMCommandHandlerBase
    {
        // \getprofile\\sesskey\19150\profileid\2\id\2\final\
        protected new GetProfileRequest _request { get { return (GetProfileRequest)base._request; } }
        protected GetProfileDBResult _result;
        public GetProfileHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
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
                             && s.Namespaceid == _session.UserData.NamespaceID
                             select new GetProfileDBResult
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
                    _errorCode = GPErrorCode.DatabaseError;
                    return;
                }

                _result = result.First();
            }
        }

        protected override void BuildNormalResponse()
        {
            base.BuildNormalResponse();
            _sendingBuffer = @"\pi\\profileid\" + _result.ProfileID;
            _sendingBuffer += @"\nick\" + _result.Nick;
            _sendingBuffer += @"\uniquenick\" + _result.UniqueNick;
            _sendingBuffer += @"\email\" + _result.Email;
            _sendingBuffer += @"\firstname\" + _result.Firstname;
            _sendingBuffer += @"\lastname\" + _result.Lastname;
            _sendingBuffer += @"\icquin\" + _result.Icquin;
            _sendingBuffer += @"\homepage\" + _result.Homepage;
            _sendingBuffer += @"\zipcode\" + _result.Zipcode;
            _sendingBuffer += @"\countrycode\" + _result.Countrycode;
            _sendingBuffer += @"\lon\" + _result.Longitude;
            _sendingBuffer += @"\lat\" + _result.Longitude;
            _sendingBuffer += @"\loc\" + _result.Location;

            int birthStr = (int)_result.Birthday << 24 | (int)_result.Birthmonth << 16 | (int)_result.Birthyear;

            _sendingBuffer += @"\birthday\" + birthStr;
            _sendingBuffer += @"\sex\" + _result.Sex;
            _sendingBuffer += @"\publicmask\" + _result.Publicmask;
            _sendingBuffer += @"\aim\" + _result.Aim;
            _sendingBuffer += @"\picture\" + _result.Picture;
            _sendingBuffer += @"\ooc" + _result.Occupationid;
            _sendingBuffer += @"\ind\" + _result.Industryid;
            _sendingBuffer += @"\inc\" + _result.Incomeid;
            _sendingBuffer += @"\mar\" + _result.Marriedid;
            _sendingBuffer += @"\chc\" + _result.Childcount;
            _sendingBuffer += @"\i1\" + _result.Interests1;
            _sendingBuffer += @"\o1\" + _result.Ownership1;
            _sendingBuffer += @"\conn\" + _result.Connectiontype;
            // SUPER NOTE: Please check the Signature of the PID, otherwise when it will be compared with other peers, it will break everything (See gpiPeer.c @ peerSig)
            _sendingBuffer += @"\sig\+" + GameSpyRandom.GenerateRandomString(10, GameSpyRandom.StringType.Hex);
            _sendingBuffer += @"\id\" + _request.OperationID + @"\final\";
        }
    }
}

