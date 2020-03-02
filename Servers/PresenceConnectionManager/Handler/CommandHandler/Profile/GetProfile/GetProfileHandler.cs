using GameSpyLib.Common;
using GameSpyLib.Database.DatabaseModel.MySql;
using PresenceConnectionManager.Enumerator;
using System.Collections.Generic;
using System.Linq;
namespace PresenceConnectionManager.Handler.Profile.GetProfile
{

    public class GetProfileHandler : GPCMHandlerBase
    {
        // \getprofile\\sesskey\19150\profileid\2\id\2\final\
        private uint _profileid;
        public GetProfileHandler(GPCMSession session, Dictionary<string, string> recv) : base(session, recv)
        {
        }

        protected override void CheckRequest(GPCMSession session, Dictionary<string, string> recv)
        {
            base.CheckRequest(session, recv);
            if (!recv.ContainsKey("profileid"))
            {
                _errorCode = GPErrorCode.Parse;
                return;
            }
            if (!recv.ContainsKey("sesskey"))
            {
                _errorCode = GPErrorCode.Parse;
                return;
            }
            if (uint.TryParse(recv["profileid"], out _profileid))
            {
                _errorCode = GPErrorCode.Parse;
                return;
            }
        }
        protected override void DataBaseOperation(GPCMSession session, Dictionary<string, string> recv)
        {
            using (var db = new retrospyContext())
            {
                var result = from p in db.Profiles
                             join s in db.Subprofiles on p.Profileid equals s.Profileid
                             join u in db.Users on p.Userid equals u.Userid
                             where p.Profileid == _profileid && s.Namespaceid == session.UserInfo.NamespaceID
                             select new
                             {
                                 nick = p.Nick,
                                 profileid = p.Profileid,
                                 uniquenick = s.Uniquenick,
                                 email = u.Email,
                                 firstname = p.Firstname,
                                 lastname = p.Lastname,
                                 icquin = p.Icquin,
                                 homepage = p.Homepage,
                                 zipcode = p.Zipcode,
                                 countrycode = p.Countrycode,
                                 longitude = p.Longitude,
                                 latitude = p.Latitude,
                                 location = p.Location,
                                 birthday = p.Birthday,
                                 birthmonth = p.Birthmonth,
                                 birthyear = p.Birthyear,
                                 sex = p.Sex,
                                 publicmask = p.Publicmask,
                                 aim = p.Aim,
                                 picture = p.Picture,
                                 occupationid = p.Occupationid,
                                 industryid = p.Industryid,
                                 incomeid = p.Incomeid,
                                 marriedid = p.Marriedid,
                                 childcount = p.Childcount,
                                 interest1 = p.Interests1,
                                 ownership1 = p.Ownership1,
                                 connectiontype = p.Connectiontype,
                             };
                if (result.Count() == 0)
                {
                    _errorCode = GPErrorCode.DatabaseError;
                    return;
                }
                var info = result.First();
                _sendingBuffer = @"\pi\\profileid\" + info.profileid;
                _sendingBuffer += @"\nick\" + info.nick;
                _sendingBuffer += @"\uniquenick\" + info.uniquenick;
                _sendingBuffer += @"\email\" + info.email;
                _sendingBuffer += @"\firstname\" + info.firstname;
                _sendingBuffer += @"\lastname\" + info.lastname;
                _sendingBuffer += @"\icquin\" + info.icquin;
                _sendingBuffer += @"\homepage\" + info.homepage;
                _sendingBuffer += @"\zipcode\" + info.zipcode;
                _sendingBuffer += @"\countrycode\" + info.countrycode;
                _sendingBuffer += @"\lon\" + info.countrycode;
                _sendingBuffer += @"\lat\" + info.latitude;
                _sendingBuffer += @"\loc\" + info.location;
                int tempbirth = (int)info.birthday << 24 | (int)info.birthmonth << 16 | (int)info.birthyear;
                _sendingBuffer += @"\birthday\" + tempbirth;
                _sendingBuffer += @"\sex\" + info.sex;
                _sendingBuffer += @"\publicmask\" + info.publicmask;
                _sendingBuffer += @"\aim\" + info.aim;
                _sendingBuffer += @"\picture\" + info.picture;
                _sendingBuffer += @"\ooc" + info.occupationid;
                _sendingBuffer += @"\ind\" + info.industryid;
                _sendingBuffer += @"\inc\" + info.incomeid;
                _sendingBuffer += @"\mar\" + info.marriedid;
                _sendingBuffer += @"\chc\" + info.childcount;
                _sendingBuffer += @"\i1\" + info.interest1;
                _sendingBuffer += @"\o1\" + info.ownership1;
                _sendingBuffer += @"\conn\" + info.connectiontype;
                // SUPER NOTE: Please check the Signature of the PID, otherwise when it will be compared with other peers, it will break everything (See gpiPeer.c @ peerSig)
                _sendingBuffer += @"\sig\+" + GameSpyRandom.GenerateRandomString(10, GameSpyRandom.StringType.Hex);
                _sendingBuffer += @"\id\" + _operationID + @"\final\";
            }
        }
    }
}
