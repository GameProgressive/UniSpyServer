using GameSpyLib.Common.Entity.Interface;
using GameSpyLib.Database.DatabaseModel.MySql;
using PresenceSearchPlayer.Enumerator;
using PresenceSearchPlayer.Handler.CommandHandler.Error;
using System.Collections.Generic;
using System.Linq;

namespace PresenceSearchPlayer.Handler.CommandHandler.UniqueSearch
{
    public class UniqueSearchHandler : PSPCommandHandlerBase
    {
        protected new UniqueSearchRequestModel _request;
        private bool _isUniquenickExist;

        public UniqueSearchHandler(ISession client, Dictionary<string, string> recv) : base(client, recv)
        {
        }

        protected override void CheckRequest()
        {
            _errorCode = _request.Parse();
        }

        protected override void DataOperation()
        {
            using (var db = new retrospyContext())
            {
                var result = from p in db.Profiles
                             join n in db.Subprofiles on p.Profileid equals n.Profileid
                             where n.Uniquenick == _request.PreferredNick
                             && n.Namespaceid == _request.NamespaceID
                             && n.Gamename == _request.GameName
                             select p.Profileid;

                if (result.Count() == 0)
                {
                    _isUniquenickExist = false;
                }
            }
        }

        protected override void ConstructResponse()
        {
            if (_errorCode != GPErrorCode.NoError)
            {
                _sendingBuffer = ErrorMsg.BuildGPErrorMsg(_errorCode);
                return;
            }

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
