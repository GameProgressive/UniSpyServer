using GameSpyLib.Abstraction.Interface;
using GameSpyLib.Database.DatabaseModel.MySql;
using PresenceSearchPlayer.Entity.Structure.Request;
using System.Collections.Generic;
using System.Linq;

namespace PresenceSearchPlayer.Handler.CommandHandler.Others
{
    internal class OthersHandlerDataModel
    {
        public uint Profileid;
        public string Nick;
        public string Uniquenick;
        public string Lastname;
        public string Firstname;
        public uint Userid;
        public string Email;
    }
    /// <summary>
    /// Get buddy's information
    /// </summary>
    public class OthersHandler : PSPCommandHandlerBase
    {
        protected OthersRequest _request;
        private List<OthersHandlerDataModel> _result;
        public OthersHandler(ISession client, Dictionary<string, string> recv) : base(client, recv)
        {
            _request = new OthersRequest(recv);
            _result = new List<OthersHandlerDataModel>();
        }

        protected override void RequestCheck()
        {
            _errorCode = _request.Parse();
        }

        protected override void DataOperation()
        {
            using (var db = new retrospyContext())
            {
                var result = from b in db.Friends
                             where b.Profileid == _request.ProfileID && b.Namespaceid == _request.NamespaceID
                             select b.Targetid;

                foreach (var info in result)
                {
                    var b = from p in db.Profiles
                            join n in db.Subprofiles on p.Profileid equals n.Profileid
                            join u in db.Users on p.Userid equals u.Userid
                            where n.Namespaceid == _request.NamespaceID
                            && n.Profileid == info && n.Gamename == _request.GameName
                            select new OthersHandlerDataModel
                            {
                                Profileid = p.Profileid,
                                Nick = p.Nick,
                                Uniquenick = n.Uniquenick,
                                Lastname = p.Lastname,
                                Firstname = p.Firstname,
                                Userid = u.Userid,
                                Email = u.Email
                            };

                    _result.Add(b.First());
                }
            }
        }


        protected override void BuildNormalResponse()
        {
            _sendingBuffer = @"\others\";

            foreach (var info in _result)
            {
                _sendingBuffer += @"\o\" + info.Profileid;
                _sendingBuffer += @"\nick\" + info.Nick;
                _sendingBuffer += @"\uniquenick\" + info.Uniquenick;
                _sendingBuffer += @"\first\" + info.Firstname;
                _sendingBuffer += @"\last\" + info.Lastname;
                _sendingBuffer += @"\email\" + info.Email;
            }
            _sendingBuffer += @"\odone\final\";
        }

    }
}
