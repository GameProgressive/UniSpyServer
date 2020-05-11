using GameSpyLib.Database.DatabaseModel.MySql;
using StatsAndTracking.Entity.Enumerator;
using System.Collections.Generic;
using System.Linq;

namespace StatsAndTracking.Handler.CommandHandler.AuthP
{
    /// <summary>
    /// Authenticate with partnerid or profileid
    /// because we are not gamespy
    /// so we do not check response string
    /// </summary>
    public class AuthPHandler : CommandHandlerBase
    {
        private uint _profileid;
        private AuthMethod _authMethod;
        public AuthPHandler() : base()
        {
            _authMethod = AuthMethod.Unknown;
        }

        protected override void CheckRequest(GStatsSession session, Dictionary<string, string> recv)
        {
            base.CheckRequest(session, recv);

            if (recv.ContainsKey("pid") && recv.ContainsKey("resp"))
            {
                //we parse profileid here
                if (!uint.TryParse(recv["pid"], out _profileid))
                {
                    _errorCode = GstatsErrorCode.Parse;
                }
                _authMethod = AuthMethod.ProfileIDAuth;
            }
            else if (recv.ContainsKey("authtoken") && recv.ContainsKey("response"))
            {
                _authMethod = AuthMethod.PartnerIDAuth;
            }
            else if (recv.ContainsKey("keyhash") && recv.ContainsKey("nick"))
            {
                _authMethod = AuthMethod.CDkeyAuth;
            }
            else
            {
                _errorCode = GstatsErrorCode.Parse;
            }


        }

        protected override void DataOperation(GStatsSession session, Dictionary<string, string> recv)
        {
            //search database for user's password
            //We do not store user's plaintext password, so we can not check this response
            switch (_authMethod)
            {
                case AuthMethod.PartnerIDAuth:
                    FindProfileByAuthtoken(recv);
                    break;
                case AuthMethod.ProfileIDAuth:
                    //even if we do not check response challenge
                    //we have to check the pid is in our databse
                    FindProfileByProfileid(recv);
                    break;
                case AuthMethod.CDkeyAuth:
                    FrindProfileByCDKeyHash(recv);
                    break;
                default:
                    _errorCode = GstatsErrorCode.Database;
                    break;
            }


        }

        protected override void ConstructResponse(GStatsSession session, Dictionary<string, string> recv)
        {
            //we did not store the plaintext of user password so we do not need to check this
            _sendingBuffer = $@"\pauthr\{_profileid}\lid\{_localId}\";
        }

        private void FindProfileByAuthtoken(Dictionary<string, string> recv)
        {
            using (var db = new retrospyContext())
            {
                var result = from s in db.Subprofiles
                             where s.Authtoken == recv["authtoken"]
                             select s.Profileid;
                if (result.Count() != 1)
                {
                    _errorCode = GstatsErrorCode.Database;
                    return;
                }
                _profileid = result.First();
            }
        }
        private void FindProfileByProfileid(Dictionary<string, string> recv)
        {
            using (var db = new retrospyContext())
            {
                var result = from p in db.Profiles
                             where p.Id == _profileid
                             select p.Id;
                if (result.Count() != 1)
                {
                    _errorCode = GstatsErrorCode.Database;
                    return;
                }
                _profileid = result.First();
            }
        }
        private void FrindProfileByCDKeyHash(Dictionary<string, string> recv)
        {
            using (var db = new retrospyContext())
            {
                var result = from s in db.Subprofiles
                             join p in db.Profiles on s.Profileid equals p.Id
                             where s.Cdkeyenc == recv["cdkeyhash"] && p.Nick == recv["nick"]
                             select s.Profileid;
                if (result.Count() != 1)
                {
                    _errorCode = GstatsErrorCode.Database;
                    return;
                }
                _profileid = result.First();
            }
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


    }
}
