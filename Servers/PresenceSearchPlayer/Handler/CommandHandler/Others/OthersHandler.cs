using GameSpyLib.Common.Entity.Interface;
using GameSpyLib.Database.DatabaseModel.MySql;
using PresenceSearchPlayer.Enumerator;
using System.Collections.Generic;
using System.Linq;

namespace PresenceSearchPlayer.Handler.CommandHandler.Others
{
    /// <summary>
    /// Get buddy's information
    /// </summary>
    public class OthersHandler : PSPCommandHandlerBase
    {


        private uint _profileid;

        public OthersHandler(IClient client, Dictionary<string, string> recv) : base(client, recv)
        {
        }

        protected override void CheckRequest()
        {

            base.CheckRequest();

            if (!_recv.ContainsKey("profileid") || !_recv.ContainsKey("namespaceid"))
            {
                _errorCode = GPErrorCode.Parse;
                return;
            }

            if (!_recv.ContainsKey("profileid") && !uint.TryParse(_recv["profileid"], out _profileid))
            {
                _errorCode = GPErrorCode.Parse;
                return;
            }

        }

        protected override void DataOperation()
        {
            using (var db = new retrospyContext())
            {
                var info = from b in db.Friends
                           where b.Profileid == _profileid && b.Namespaceid == _namespaceid
                           select b.Targetid;

                _sendingBuffer = @"\others\";

                foreach (var pid in info)
                {
                    var b = from p in db.Profiles
                            join n in db.Subprofiles on p.Profileid equals n.Profileid
                            join u in db.Users on p.Userid equals u.Userid
                            where n.Namespaceid == _namespaceid && n.Profileid == pid && n.Gamename == _recv["gamename"]
                            select new
                            {
                                profileid = p.Profileid,
                                nick = p.Nick,
                                uniquenick = n.Uniquenick,
                                last = p.Lastname,
                                first = p.Firstname,
                                email = u.Userid
                            };

                    var result = b.FirstOrDefault();

                    _sendingBuffer += @"\o\" + result.profileid;
                    _sendingBuffer += @"\nick\" + result.nick;
                    _sendingBuffer += @"\uniquenick\" + result.uniquenick;
                    _sendingBuffer += @"\first\" + result.first;
                    _sendingBuffer += @"\last\" + result.last;
                    _sendingBuffer += @"\email\" + result.email;
                }

                _sendingBuffer += @"\odone\final\";
            }
        }
    }
}
