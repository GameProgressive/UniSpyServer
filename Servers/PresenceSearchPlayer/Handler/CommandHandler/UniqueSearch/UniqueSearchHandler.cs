using GameSpyLib.Common.Entity.Interface;
using GameSpyLib.Database.DatabaseModel.MySql;
using PresenceSearchPlayer.Enumerator;
using System.Collections.Generic;
using System.Linq;

namespace PresenceSearchPlayer.Handler.CommandHandler.UniqueSearch
{
    public class UniqueSearchHandler : PSPCommandHandlerBase
    {
        private bool _isUniquenickExist;

        public UniqueSearchHandler(ISession client, Dictionary<string, string> recv) : base(client, recv)
        {
        }

        protected override void CheckRequest()
        {
            base.CheckRequest();

            if (!_recv.ContainsKey("preferrednick"))
            {
                _errorCode = GPErrorCode.Parse;
            }
        }

        protected override void DataOperation()
        {
            using (var db = new retrospyContext())
            {
                var result = from p in db.Profiles
                             join n in db.Subprofiles on p.Profileid equals n.Profileid
                             where n.Uniquenick == _recv["preferrednick"]
                             && n.Namespaceid == _namespaceid
                             && n.Gamename == _recv["gamename"]
                             select p.Profileid;

                if (result.Count() == 0)
                {
                    _isUniquenickExist = false;
                }
            }
        }

        protected override void ConstructResponse()
        {
            if (!_isUniquenickExist)
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
