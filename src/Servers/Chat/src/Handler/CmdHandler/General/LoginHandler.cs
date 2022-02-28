using UniSpyServer.Servers.Chat.Abstraction.BaseClass;
using UniSpyServer.Servers.Chat.Entity.Contract;
using UniSpyServer.Servers.Chat.Entity.Exception;
using UniSpyServer.Servers.Chat.Entity.Structure.Request.General;
using UniSpyServer.Servers.Chat.Entity.Structure.Response.General;
using UniSpyServer.Servers.Chat.Entity.Structure.Result.General;
using System.Linq;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Database.DatabaseModel;

namespace UniSpyServer.Servers.Chat.Handler.CmdHandler.General
{
    [HandlerContract("LOGIN")]
    public sealed class LoginHandler : CmdHandlerBase
    {

        private new LoginRequest _request => (LoginRequest)base._request;
        private new LoginResult _result{ get => (LoginResult)base._result; set => base._result = value; }
        public LoginHandler(IClient client, IRequest request) : base(client, request)
        {
            _result = new LoginResult();
        }

        protected override void RequestCheck()
        {
            /// TODO: Verify which games does send a GS encoded password and not md5
            //we decoded gamespy encoded password then get md5 of it 
            //_password = GameSpyUtils.DecodePassword(_request.PasswordHash);
            //_password = StringExtensions.GetMD5Hash(_password);
            base.RequestCheck();
        }

        protected override void DataOperation()
        {
            switch (_request.ReqeustType)
            {
                case LoginReqeustType.NickAndEmailLogin:
                    NickAndEmailLogin();
                    break;
                case LoginReqeustType.UniqueNickLogin:
                    UniqueNickLogin();
                    break;
            }
        }
        public void NickAndEmailLogin()
        {
            using (var db = new UniSpyContext())
            {
                var result = from u in db.Users
                            join p in db.Profiles on u.Userid equals p.Userid
                            where u.Email == _request.Email
                            && p.Nick == _request.NickName
                            && u.Password == _request.PasswordHash
                            select new
                            {
                                userid = u.Userid,
                                profileid = p.ProfileId,
                                emailVerified = u.Emailverified,
                                banned = u.Banned
                            };

                if (result.Count() != 1)
                {
                    throw new Exception($"Can not find user with nickname:{_request.NickName} in database.");
                }
                _result.ProfileId = result.First().profileid;
                _result.UserID = result.First().userid;
            }
        }
        public void UniqueNickLogin()
        {
            using (var db = new UniSpyContext())
            {
                var result = from n in db.Subprofiles
                            join p in db.Profiles on n.ProfileId equals p.ProfileId
                            join u in db.Users on p.Userid equals u.Userid
                            where n.Uniquenick == _request.UniqueNick
                            && n.Namespaceid == _request.NamespaceID
                            select new
                            {
                                userid = u.Userid,
                                profileid = p.ProfileId,
                                uniquenick = n.Uniquenick,
                                emailVerified = u.Emailverified,
                                banned = u.Banned
                            };
                if (result.Count() != 1)
                {
                    throw new Exception($"Can not find user with uniquenick:{_request.UniqueNick} in database.");
                }
                _result.ProfileId = result.First().profileid;
                _result.UserID = result.First().userid;
            }
        }

        protected override void ResponseConstruct()
        {
            _response = new LoginResponse(_request, _result);
        }
    }
}
