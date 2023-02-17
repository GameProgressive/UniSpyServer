using UniSpy.Server.GameStatus.Abstraction.BaseClass;
using UniSpy.Server.GameStatus.Entity.Enumerate;
using UniSpy.Server.GameStatus.Entity.Exception;
using UniSpy.Server.GameStatus.Entity.Structure.Request;
using UniSpy.Server.GameStatus.Entity.Structure.Response;
using UniSpy.Server.GameStatus.Entity.Structure.Result;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Core.Database.DatabaseModel;
using UniSpy.Server.GameStatus.Application;

namespace UniSpy.Server.GameStatus.Handler.CmdHandler
{
    /// <summary>
    /// Authenticate with partnerid or profileid
    /// because we are not gamespy
    /// so we do not check response string
    /// </summary>

    public sealed class AuthPlayerHandler : CmdHandlerBase
    {
        private new AuthPlayerRequest _request => (AuthPlayerRequest)base._request;
        private new AuthPlayerResult _result { get => (AuthPlayerResult)base._result; set => base._result = value; }
        public AuthPlayerHandler(IClient client, IRequest request) : base(client, request)
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
                        StorageOperation.Persistance.GetProfileId(_request.AuthToken);
                        break;
                    case AuthMethod.ProfileIDAuth:
                        //even if we do not check response challenge
                        //we have to check the pid is in our databse
                        StorageOperation.Persistance.GetProfileId(_request.ProfileId);
                        break;
                    case AuthMethod.CDkeyAuth:
                        StorageOperation.Persistance.GetProfileId(_request.CdKeyHash, _request.NickName);
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


        ////request \authp\\pid\27\resp\16ae1e1f47c8ab646de7a52d615e3b06\lid\0\final\
        //public static void AuthPlayer(GStatsSession connection, Dictionary<string, string> dict)
        //{
        //    /*
        //     *process the playerauth result 
        //     first, check \resp\16ae1e1f47c8ab646de7a52d615e3b06
        //     then find the 
        //     */

        //    //connection.SendAsync(@"\pauthr\26\lid\"+dict["lid"]);
        //    //connection.SendAsync(@"\getpidr\26\lid\" + dict["lid"]);
        //    //connection.SendAsync(@"\pauthr\26\lid\" + dict["lid"]);
        //    //connection.SendAsync(@" \getpdr\26\lid\"+dict["lid"]+@"\mod\1234\length\5\data\mydata");
        //    //connection.SendAsync(@"\setpdr\1\lid\"+dict["lid"]+@"\pid\26\mod\123");
        //}


    }
}
