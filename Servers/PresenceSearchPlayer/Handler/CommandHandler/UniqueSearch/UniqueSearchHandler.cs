using GameSpyLib.Common;
using GameSpyLib.Database.DatabaseModel.MySql;
using PresenceSearchPlayer.Enumerator;
using System.Collections.Generic;
using System.Linq;

namespace PresenceSearchPlayer.Handler.CommandHandler.UniqueSearch
{
    public class UniqueSearchHandler:GPSPHandlerBase
    {
        public  UniqueSearchHandler(Dictionary<string, string> recv) : base(recv)
        {
        }
        bool IsUniquenickExist; 
        
        protected override void CheckRequest(GPSPSession session)
        {
            base.CheckRequest(session);
            if (!_recv.ContainsKey("preferrednick"))
            {
                _errorCode = GPErrorCode.Parse;
            }
        }

        protected override void DataBaseOperation(GPSPSession session)
        {
            using (var db = new RetrospyDB())
            {
                var result = from p in db.Profiles
                                    join n in db.Subprofiles on p.Profileid equals n.Profileid
                                    where n.Uniquenick == _recv["preferrednick"] && n.Namespaceid == _namespaceid && n.Gamename == _recv["gamename"]
                                    select p.Profileid;
                if (result.Count() == 0)
                {
                    IsUniquenickExist = false;
                }
            }
           
        }
        protected override void ConstructResponse(GPSPSession session)
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
