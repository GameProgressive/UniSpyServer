using GameSpyLib.Common;
using GameSpyLib.Database.DatabaseModel.MySql;
using PresenceConnectionManager.Enumerator;
using System;
using System.Collections.Generic;
using System.Linq;
using LinqToDB;

namespace PresenceConnectionManager.Handler.Profile.UpdatePro
{
    public class UpdateProHandler : GPCMHandlerBase
    {
        protected UpdateProHandler(Dictionary<string, string> recv) : base(recv)
        {
        }

        protected override void CheckRequest(GPCMSession session)
        {
            base.CheckRequest(session);
        }

        protected override void DataBaseOperation(GPCMSession session)
        {
            using (var db = new RetrospyDB())
            {
                var profile = db.Profiles.Where(
                    p => p.Userid == session.UserInfo.Userid
                    && p.Profileid == session.UserInfo.Profileid
                    && p.Nick == p.Nick).First();

                var user = db.Users.Where(
                    u => u.Userid == session.UserInfo.Userid).First();

                var subprofile = db.Subprofiles.Where(
                    s => s.Profileid == session.UserInfo.Profileid
                    && s.Namespaceid == session.UserInfo.NamespaceID
                    && s.Uniquenick == session.UserInfo.UniqueNick).First();


                if (_recv.ContainsKey("publicmask"))
                {
                    PublicMasks mask;
                    if (Enum.TryParse(_recv["publicmask"], out mask))
                    {
                        profile.Publicmask = (int)mask;
                    }
                }

                if (_recv.ContainsKey("firstname"))
                {
                    profile.Firstname = _recv["firstname"];
                }

                if (_recv.ContainsKey("lastname"))
                {
                    profile.Lastname = _recv["lastname"];
                }

                if (_recv.ContainsKey("icquin"))
                {
                    uint icq;
                    uint.TryParse(_recv["icquin"], out icq);
                    profile.Icquin = icq;
                }

                if (_recv.ContainsKey("homepage"))
                {
                    profile.Homepage = _recv["homepage"];
                }

                if (_recv.ContainsKey("birthday"))
                {
                    int date;
                    if (int.TryParse(_recv["birthday"], out date))
                    {
                        int d = (int)((date >> 24) & 0xFF);
                        ushort m = (ushort)((date >> 16) & 0xFF);
                        ushort y = (ushort)(date & 0xFFFF);

                        if (GameSpyUtils.IsValidDate(d, m, y))
                        {
                            profile.Birthday = d;
                            profile.Birthmonth = m;
                            profile.Birthyear = y;
                        }
                    }
                    if (_recv.ContainsKey("sex"))
                    {
                        byte sex;
                        if (byte.TryParse(_recv["sex"], out sex))
                        {
                            profile.Sex = Convert.ToSByte(_recv["sex"]);
                        }

                    }
                    if (_recv.ContainsKey("zipcode"))
                    {

                        profile.Zipcode = _recv["zipcode"];
                    }

                    if (_recv.ContainsKey("countrycode"))
                    {
                        profile.Countrycode = _recv["countrycode"];
                    }
                    db.Update(profile);
                }
            }
        }
    }
}

