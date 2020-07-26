using GameSpyLib.Common.Entity.Interface;
using GameSpyLib.Database.DatabaseModel.MySql;
using System.Collections.Generic;
using System.Linq;

namespace PresenceConnectionManager.Handler.Profile.NewProfile
{
    public class NewProfileHandler : PCMCommandHandlerBase
    {
        public NewProfileHandler(ISession client, Dictionary<string, string> recv) : base(client, recv)
        {
        }

        //create a new profile with new nick 
        // @"  \newprofile\sesskey\<>\nick\<>\id\1\final\"
        //replace a existed nick with new nick
        //@"  \newprofile\sesskey\<>\nick\<>\replace\1\oldnick\<>\id\1\final\"

        protected override void CheckRequest()
        {
            base.CheckRequest();
        }

        protected override void DataOperation()
        {
            using (var db = new retrospyContext())
            {
                if (_recv.ContainsKey("replace"))
                {
                    db.Profiles.Where(p => p.Profileid == _session.UserInfo.ProfileID
                    && p.Nick == _recv["oldnick"]).First().Nick = _recv["nick"];
                    db.SaveChanges();
                }
                else
                {
                    Profiles profiles = new Profiles
                    {
                        Profileid = _session.UserInfo.ProfileID,
                        Nick = _recv["nick"],
                        Userid = _session.UserInfo.UserID
                    };
                    db.Add(profiles);
                }
            }
        }
    }
}
