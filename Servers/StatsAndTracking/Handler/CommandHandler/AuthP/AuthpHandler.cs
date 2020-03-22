using System.Collections.Generic;
using GameSpyLib.Database.DatabaseModel.MySql;
using System.Linq;

namespace StatsAndTracking.Handler.CommandHandler.AuthP
{
    /// <summary>
    /// Authenticate with profileid
    /// </summary>
    public class AuthPHandler:CommandHandlerBase
    {
        private uint _profileid;

        public AuthPHandler(GStatsSession session, Dictionary<string, string> recv) : base(session, recv)
        {
        }

        protected override void CheckRequest(GStatsSession session, Dictionary<string, string> recv)
        {
            base.CheckRequest(session, recv);

            if (recv.ContainsKey("pid"))
            {
                if (!uint.TryParse(recv["pid"], out _profileid))
                {
                    _errorCode = Entity.Enumerator.GstatsErrorCode.Parse;
                }
            }
            else
            {
                _errorCode = Entity.Enumerator.GstatsErrorCode.Parse;
            }
        }

        protected override void ConstructResponse(GStatsSession session, Dictionary<string, string> recv)
        {
            //we did not store the plaintext of user password so we do not need to check this
            _sendingBuffer =$@"\pauthr\{_profileid}\lid\{_localId}\";
        }

        ////request \authp\\pid\27\resp\16ae1e1f47c8ab646de7a52d615e3b06\lid\0\final\
        //public static void AuthPlayer(GStatsSession session, Dictionary<string, string> dict)
        //{
        //    /*
        //     *process the playerauth result 
        //     first, check \resp\16ae1e1f47c8ab646de7a52d615e3b06
        //     then find the 
        //     */

        //    //session.SendAsync(@"\pauthr\26\lid\"+dict["lid"]);
        //    //session.SendAsync(@"\getpidr\26\lid\" + dict["lid"]);
        //    //session.SendAsync(@"\pauthr\26\lid\" + dict["lid"]);
        //    //session.SendAsync(@" \getpdr\26\lid\"+dict["lid"]+@"\mod\1234\length\5\data\mydata");
        //    //session.SendAsync(@"\setpdr\1\lid\"+dict["lid"]+@"\pid\26\mod\123");
        //}

        protected override void DataOperation(GStatsSession session, Dictionary<string, string> recv)
        {  
            //search database for user's password
            //We do not store user's plaintext password, so we can not check this response
            //using (var db = new RetrospyDB())
            //{
            //    var result = from u in db.Users
            //               join p in db.Profiles on u.Userid equals p.Userid
            //               where p.Profileid == _profileid
            //               select new { u.Password };
            //    _responseStr = result.FirstOrDefault().Password + session.Challenge;
            //}
            //_responseStr = GameSpyLib.Encryption.StringExtensions.GetMD5Hash(_responseStr);
        }
    }
}
