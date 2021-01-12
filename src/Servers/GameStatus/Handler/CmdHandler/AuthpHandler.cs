using GameStatus.Abstraction.BaseClass;
using GameStatus.Entity.Enumerate;
using GameStatus.Entity.Structure.Request;
using GameStatus.Entity.Structure.Response;
using GameStatus.Entity.Structure.Result;
using System.Collections.Generic;
using System.Linq;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Database.DatabaseModel.MySql;

namespace GameStatus.Handler.CmdHandler
{
    /// <summary>
    /// Authenticate with partnerid or profileid
    /// because we are not gamespy
    /// so we do not check response string
    /// </summary>
    internal sealed class AuthPHandler : GSCmdHandlerBase
    {
        private new AuthPRequest _request => (AuthPRequest)base._request;
        private new AuthPResult _result
        {
            get { return (AuthPResult)base._result; }
            set { base._result = value; }
        }
        public AuthPHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }
        protected override void RequestCheck()
        {
            _result = new AuthPResult();
        }
        protected override void DataOperation()
        {
            //search database for user's password
            //We do not store user's plaintext password, so we can not check this response
            using (var db = new retrospyContext())
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
                        _errorCode = GSErrorCode.Database;
                        break;
                }
            }

        }

        protected override void ResponseConstruct()
        {
            _response = new AuthPResponse(_request, _result);
            //we did not store the plaintext of user password so we do not need to check this
            _sendingBuffer = $@"\pauthr\{_result.ProfileID}\lid\{ _request.OperationID}";
        }

        private void FindProfileByAuthtoken()
        {
            using (var db = new retrospyContext())
            {
                var result = from s in db.Subprofiles
                             where s.Authtoken == _request.AuthToken
                             select s.Profileid;
                if (result.Count() != 1)
                {
                    _errorCode = GSErrorCode.Database;
                    return;
                }
                _result.ProfileID = result.First();
            }
        }
        private void FindProfileByProfileid()
        {
            using (var db = new retrospyContext())
            {
                var result = from p in db.Profiles
                             where p.Profileid == _request.ProfileID
                             select p.Profileid;
                if (result.Count() != 1)
                {
                    _errorCode = GSErrorCode.Database;
                    return;
                }
                _result.ProfileID = result.First();
            }
        }
        private void FrindProfileByCDKeyHash()
        {
            using (var db = new retrospyContext())
            {
                var result = from s in db.Subprofiles
                             join p in db.Profiles on s.Profileid equals p.Profileid
                             where s.Cdkeyenc == _request.KeyHash && p.Nick == _request.Nick
                             select s.Profileid;
                if (result.Count() != 1)
                {
                    _errorCode = GSErrorCode.Database;
                    return;
                }
                _result.ProfileID = result.First();
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
