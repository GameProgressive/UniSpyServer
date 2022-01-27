using UniSpyServer.Servers.GameStatus.Abstraction.BaseClass;
using UniSpyServer.Servers.GameStatus.Entity.Contract;
using UniSpyServer.Servers.GameStatus.Entity.Enumerate;
using UniSpyServer.Servers.GameStatus.Entity.Exception;
using UniSpyServer.Servers.GameStatus.Entity.Structure.Request;
using UniSpyServer.Servers.GameStatus.Entity.Structure.Response;
using UniSpyServer.Servers.GameStatus.Entity.Structure.Result;
using System.Linq;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Database.DatabaseModel;

namespace UniSpyServer.Servers.GameStatus.Handler.CmdHandler
{
    /// <summary>
    /// Authenticate with partnerid or profileid
    /// because we are not gamespy
    /// so we do not check response string
    /// </summary>
    [HandlerContract("authp")]
    public sealed class AuthPlayerHandler : CmdHandlerBase
    {
        private new AuthPlayerRequest _request => (AuthPlayerRequest)base._request;
        private new AuthPlayerResult _result { get => (AuthPlayerResult)base._result; set => base._result = value; }
        public AuthPlayerHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _result = new AuthPlayerResult();
        }
        protected override void DataOperation()
        {
            //search database for user's password
            //We do not store user's plaintext password, so we can not check this response
            using (var db = new UniSpyContext())
            {
                switch (_request.RequestType)
                {
                    case AuthMethod.PartnerIDAuth:
                        FindProfileByAuthtoken();
                        break;
                    case AuthMethod.ProfileIDAuth:
                        //even if we do not check response challenge
                        //we have to check the pid is in our databse
                        FindProfileByProfileid();
                        break;
                    case AuthMethod.CDkeyAuth:
                        FrindProfileByCDKeyHash();
                        break;
                    default:
                        throw new GSException("Unknown AuthP request type.");
                }
            }

        }

        protected override void ResponseConstruct()
        {
            _response = new AuthPlayerResponse(_request, _result);
            //we did not store the plaintext of user password so we do not need to check this
        }

        private void FindProfileByAuthtoken()
        {
            using (var db = new UniSpyContext())
            {
                var result = from s in db.Subprofiles
                             where s.Authtoken == _request.AuthToken
                             select s.ProfileId;
                if (result.Count() != 1)
                {
                    throw new GSException("No records found in database by authtoken.");
                }
                _result.ProfileId = result.First();
            }
        }
        private void FindProfileByProfileid()
        {
            using (var db = new UniSpyContext())
            {
                var result = from p in db.Profiles
                             where p.ProfileId == _request.ProfileId
                             select p.ProfileId;
                if (result.Count() != 1)
                {
                    throw new GSException("No records found in database by profileid.");
                }
                _result.ProfileId = result.First();
            }
        }
        private void FrindProfileByCDKeyHash()
        {
            using (var db = new UniSpyContext())
            {
                var result = from s in db.Subprofiles
                             join p in db.Profiles on s.ProfileId equals p.ProfileId
                             where s.Cdkeyenc == _request.KeyHash && p.Nick == _request.Nick
                             select s.ProfileId;
                if (result.Count() != 1)
                {
                    throw new GSException("No records found in database by cdkey hash.");
                }
                _result.ProfileId = result.First();
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
