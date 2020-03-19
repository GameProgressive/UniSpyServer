using GameSpyLib.Database.DatabaseModel.MySql;
using PresenceSearchPlayer.Enumerator;
using System.Collections.Generic;
using System.Linq;

namespace PresenceSearchPlayer.Handler.CommandHandler.UniqueSearch
{
    public class UniqueSearchHandler : CommandHandlerBase
    {
        public UniqueSearchHandler(GPSPSession session, Dictionary<string, string> recv) : base(session, recv)
        {
        }

        private bool IsUniquenickExist;

        protected override void CheckRequest(GPSPSession session, Dictionary<string, string> recv)
        {
            base.CheckRequest(session, recv);

            if (!recv.ContainsKey("preferrednick"))
            {
                _errorCode = GPErrorCode.Parse;
            }
        }

        protected override void DataOperation(GPSPSession session, Dictionary<string, string> recv)
        {
            using (var db = new retrospyContext())
            {
                var result = from p in db.Profiles
                             join n in db.Subprofiles on p.Profileid equals n.Profileid
                             where n.Uniquenick == recv["preferrednick"] && n.Namespaceid == _namespaceid && n.Gamename == recv["gamename"]
                             select p.Profileid;

                if (result.Count() == 0)
                {
                    IsUniquenickExist = false;
                }
            }
        }

        protected override void ConstructResponse(GPSPSession session, Dictionary<string, string> recv)
        {
            if (!IsUniquenickExist)
            {
                _sendingBuffer = @"us\0\usdone\final";
            }
            else
            {
                _sendingBuffer = @"\us\1\nick\choose another name\usdone\final\";
            }
        }
    }
}
