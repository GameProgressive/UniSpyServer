using GameSpyLib.Database.DatabaseModel.MySql;
using PresenceSearchPlayer.Enumerator;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PresenceSearchPlayer.Handler.CommandHandler.SearchUnique
{
    /// <summary>
    /// Search with uniquenick and namespace
    /// </summary>
    public class SearchUniqueHandler : GPSPHandlerBase
    {
        public SearchUniqueHandler(GPSPSession session, Dictionary<string, string> recv) : base(session, recv)
        {
        }

        protected override void CheckRequest(GPSPSession session, Dictionary<string, string> recv)
        {
            base.CheckRequest(session, recv);
            if (!recv.ContainsKey("uniquenick") || !recv.ContainsKey("namespaces"))
            {
                _errorCode = GPErrorCode.Parse;
            }
        }

        protected override void DataOperation(GPSPSession session, Dictionary<string, string> recv)
        {
            string[] tempstr = recv["namespaces"].Trim(',').Split(',');
            uint[] nspaceid = Array.ConvertAll(tempstr, uint.Parse);

            using (var db = new retrospyContext())
            {
                foreach (var id in nspaceid)
                {
                    var result = from p in db.Profiles
                                 join n in db.Subprofiles on p.Profileid equals n.Profileid
                                 join u in db.Users on p.Userid equals u.Userid
                                 where n.Uniquenick == recv["uniquenick"]
                                 && n.Namespaceid == _namespaceid
                                 select new
                                 {
                                     profileid = n.Profileid,
                                     nick = p.Nick,
                                     uniquenick = n.Uniquenick,
                                     email = u.Email,
                                     first = p.Firstname,
                                     last = p.Lastname
                                 };
                    var info = result.First();
                    _sendingBuffer = @"\bsr\" + info.profileid;
                    _sendingBuffer += @"\nick\" + info.nick;
                    _sendingBuffer += @"\uniquenick\" + info.uniquenick;
                    _sendingBuffer += @"\namespaceid\" + _namespaceid;
                    _sendingBuffer += @"\firstname\" + info.first;
                    _sendingBuffer += @"\lastname\" + info.last;
                    _sendingBuffer += @"\email\" + info.email;
                }
                _sendingBuffer += @"\bsrdone\\more\0\final\";
            }
        }
    }
}

