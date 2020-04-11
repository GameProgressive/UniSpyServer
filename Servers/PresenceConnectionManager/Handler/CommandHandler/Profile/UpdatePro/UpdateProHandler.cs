using GameSpyLib.Common;
using GameSpyLib.Database.DatabaseModel.MySql;
using GameSpyLib.MiscMethod;
using PresenceConnectionManager.Enumerator;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PresenceConnectionManager.Handler.Profile.UpdatePro
{
    public class UpdateProHandler : CommandHandlerBase
    {
        public UpdateProHandler() : base()
        {
        }

        protected override void CheckRequest(GPCMSession session, Dictionary<string, string> recv)
        {
            base.CheckRequest(session, recv);
        }

        protected override void DataOperation(GPCMSession session, Dictionary<string, string> recv)
        {
            using (var db = new retrospyContext())
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

                if (recv.ContainsKey("publicmask"))
                {
                    PublicMasks mask;
                    if (Enum.TryParse(recv["publicmask"], out mask))
                    {
                        profile.Publicmask = (int)mask;
                    }
                }

                if (recv.ContainsKey("firstname"))
                {
                    profile.Firstname = recv["firstname"];
                }

                if (recv.ContainsKey("lastname"))
                {
                    profile.Lastname = recv["lastname"];
                }

                if (recv.ContainsKey("icquin"))
                {
                    uint icq;

                    uint.TryParse(recv["icquin"], out icq);
                    profile.Icquin = icq;
                }

                if (recv.ContainsKey("homepage"))
                {
                    profile.Homepage = recv["homepage"];
                }

                if (recv.ContainsKey("birthday"))
                {
                    int date;

                    if (int.TryParse(recv["birthday"], out date))
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

                    if (recv.ContainsKey("sex"))
                    {
                        byte sex;

                        if (byte.TryParse(recv["sex"], out sex))
                        {
                            profile.Sex = Convert.ToByte(recv["sex"]);
                        }
                    }

                    if (recv.ContainsKey("zipcode"))
                    {
                        profile.Zipcode = recv["zipcode"];
                    }

                    if (recv.ContainsKey("countrycode"))
                    {
                        profile.Countrycode = recv["countrycode"];
                    }
                    db.Update(profile);
                }
            }
        }
    }
}
