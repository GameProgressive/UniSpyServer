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
        protected NewProfileHandler(Dictionary<string, string> recv) : base(recv)
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
                if (_recv.ContainsKey("replace"))
                {
                    db.Profiles.Where(p => p.Profileid == session.UserInfo.Profileid && p.Nick == _recv["oldnick"])
                        .Set(p => p.Nick, _recv["nick"])
                        .Update();
                }
                else
                {
                    db.Profiles
                            .Value(p => p.Profileid, session.UserInfo.Profileid)
                            .Value(p => p.Nick, _recv["nick"])
                            .Value(p => p.Userid, session.UserInfo.Userid)
                            .Insert();
                }
            }
        }

    }
}
