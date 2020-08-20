using System.Linq;
using Chat.Entity.Structure;
using Chat.Entity.Structure.ChatCommand;
using Chat.Entity.Structure.ChatResponse;
using GameSpyLib.Common.Entity.Interface;
using GameSpyLib.Database.DatabaseModel.MySql;
using GameSpyLib.Extensions;
using GameSpyLib.MiscMethod;

namespace Chat.Handler.CommandHandler
{
    public class LOGINHandler : ChatCommandHandlerBase
    {

        string _password;
        new LOGIN _cmd;
        uint _profileid;
        uint _userid;
        public LOGINHandler(ISession session, ChatCommandBase cmd) : base(session, cmd)
        {
            _cmd = (LOGIN)cmd;
        }

        public override void CheckRequest()
        {
            base.CheckRequest();

            /// TODO: Verify which games does send a GS encoded password and not md5
            //we decoded gamespy encoded password then get md5 of it 
            //_password = GameSpyUtils.DecodePassword(_cmd.PasswordHash);
            //_password = StringExtensions.GetMD5Hash(_password);

            _password = _cmd.PasswordHash;
        }

        public override void DataOperation()
        {
            base.DataOperation();
            switch (_cmd.RequestType)
            {
                case LoginType.NickAndEmailLogin:
                    NickAndEmailLogin();
                    break;
                case LoginType.UniqueNickLogin:
                    UniqueNickLogin();
                    break;
            }

        }

        public override void ConstructResponse()
        {
            base.ConstructResponse();
            _sendingBuffer = ChatReply.BuildLoginReply(_userid, _profileid);
        }


        public void NickAndEmailLogin()
        {
            using (var db = new retrospyContext())
            {
                var result = from u in db.Users
                             join p in db.Profiles on u.Userid equals p.Userid
                             where u.Email == _cmd.Email
                             && p.Nick == _cmd.NickName
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
                           where n.Uniquenick == _cmd.UniqueNick
                           && n.Namespaceid == _cmd.NameSpaceID
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
