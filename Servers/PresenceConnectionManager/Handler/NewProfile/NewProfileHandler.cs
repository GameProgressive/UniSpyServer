using System;
using System.Collections.Generic;
using System.Text;

namespace PresenceConnectionManager.Handler.NewProfile
{
    public class NewProfileHandler
    {
        public static void ReplaceNick(GPCMSession session, Dictionary<string,string> recv)
        {
            //create a new profile with new nick 
           // @"  \newprofile\sesskey\<>\nick\<>\id\1\final\"
            //replace a existed nick with new nick
            //@"  \newprofile\sesskey\<>\nick\<>\replace\1\oldnick\<>\id\1\final\"


        }
    }
}
