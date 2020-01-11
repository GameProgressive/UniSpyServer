using System;
using System.Collections.Generic;

namespace StatsAndTracking.Handler.CommandHandler.AuthP
{
    /// <summary>
    /// Authenticate with profileid
    /// </summary>
    public class AuthPHandler
    {
        //request \authp\\pid\27\resp\16ae1e1f47c8ab646de7a52d615e3b06\lid\0\final\
        public static void AuthPlayer(GstatsSession session, Dictionary<string, string> dict)
        {
            /*
             *process the playerauth result 
             first, check \resp\16ae1e1f47c8ab646de7a52d615e3b06
             then find the 
             */

            //session.SendAsync(@"\pauthr\26\lid\"+dict["lid"]);
            session.SendAsync(@"\getpidr\26\lid\" + dict["lid"]);
            //session.SendAsync(@"\pauthr\26\lid\" + dict["lid"]);
            //session.SendAsync(@" \getpdr\26\lid\"+dict["lid"]+@"\mod\1234\length\5\data\mydata");
            //session.SendAsync(@"\setpdr\1\lid\"+dict["lid"]+@"\pid\26\mod\123");
        }
    }
}
