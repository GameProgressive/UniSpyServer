using GameSpyLib.Database.DatabaseModel.MySql;
using LinqToDB;
using System.Collections.Generic;
using System.Linq;

namespace PresenceConnectionManager.Handler.Profile.NewProfile
{
    public class NewProfileHandler : GPCMHandlerBase
    {
        //create a new profile with new nick 
        // @"  \newprofile\sesskey\<>\nick\<>\id\1\final\"
        //replace a existed nick with new nick
        //@"  \newprofile\sesskey\<>\nick\<>\replace\1\oldnick\<>\id\1\final\"
        public NewProfileHandler(GPCMSession session, Dictionary<string, string> recv) : base(session, recv)
        {
        }

        protected override void CheckRequest(GPCMSession session, Dictionary<string, string> recv)
        {
            base.CheckRequest(session, recv);
        }

        protected override void DataBaseOperation(GPCMSession session, Dictionary<string, string> recv)
        {
            using (var db = new RetrospyDB())
            {
                if (recv.ContainsKey("replace"))
                {
                    db.Profiles.Where(p => p.Profileid == session.UserInfo.Profileid && p.Nick == recv["oldnick"])
                        .Set(p => p.Nick, recv["nick"])
                        .Update();
                }
                else
                {
                    db.Profiles
                            .Value(p => p.Profileid, session.UserInfo.Profileid)
                            .Value(p => p.Nick, recv["nick"])
                            .Value(p => p.Userid, session.UserInfo.Userid)
                            .Insert();
                }
            }
        }

    }
}
