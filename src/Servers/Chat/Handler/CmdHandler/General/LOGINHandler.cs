using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure;
using Chat.Entity.Structure.ChatCommand;
using Chat.Entity.Structure.Response.General;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Database.DatabaseModel.MySql;
using System.Linq;

namespace Chat.Handler.CmdHandler.ChatGeneralCommandHandler
{
    public class LOGINHandler : ChatCmdHandlerBase
    {

        string _password;
        protected new LOGINRequest _request { get { return (LOGINRequest)base._request; } }
        uint _profileid;
        uint _userid;
        public LOGINHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        protected override void CheckRequest()
        {
            base.CheckRequest();
            /// TODO: Verify which games does send a GS encoded password and not md5
            //we decoded gamespy encoded password then get md5 of it 
            //_password = GameSpyUtils.DecodePassword(_request.PasswordHash);
            //_password = StringExtensions.GetMD5Hash(_password);

            _password = _request.PasswordHash;
        }

        protected override void DataOperation()
        {
            base.DataOperation();
            switch (_request.RequestType)
            {
                case LoginType.NickAndEmailLogin:
                    NickAndEmailLogin();
                    break;
                case LoginType.UniqueNickLogin:
                    UniqueNickLogin();
                    break;
            }

        }

        protected override void BuildNormalResponse()
        {
            base.BuildNormalResponse();
            _sendingBuffer = LOGINReply.BuildLoginReply(_userid, _profileid);
        }


        public void NickAndEmailLogin()
        {
            using (var db = new retrospyContext())
            {
                var result = from u in db.Users
                             join p in db.Profiles on u.Userid equals p.Userid
                             where u.Email == _request.Email
                             && p.Nick == _request.NickName
                             && u.Password == _password
                             select new
                             {
                                 userid = u.Userid,
                                 profileid = p.Profileid,
                                 emailVerified = u.Emailverified,
                                 banned = u.Banned
                             };

                if (result.Count() != 1)
                {
                    _errorCode = ChatError.DataOperation;
                    return;
                }
                _profileid = result.First().profileid;
                _userid = result.First().userid;
            }
        }
        public void UniqueNickLogin()
        {
            using (var db = new retrospyContext())
            {
                var result = from n in db.Subprofiles
                             join p in db.Profiles on n.Profileid equals p.Profileid
                             join u in db.Users on p.Userid equals u.Userid
                             where n.Uniquenick == _request.UniqueNick
                             && n.Namespaceid == _request.NameSpaceID
                             select new
                             {
                                 userid = u.Userid,
                                 profileid = p.Profileid,
                                 uniquenick = n.Uniquenick,
                                 emailVerified = u.Emailverified,
                                 banned = u.Banned
                             };
                if (result.Count() != 1)
                {
                    _errorCode = ChatError.DataOperation;
                    return;
                }
                _profileid = result.First().profileid;
                _userid = result.First().userid;
            }
        }
    }
}
