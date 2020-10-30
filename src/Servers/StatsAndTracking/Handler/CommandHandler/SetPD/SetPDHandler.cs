using GameSpyLib.Abstraction.Interface;
using GameSpyLib.Database.DatabaseModel.MySql;
using StatsAndTracking.Abstraction.BaseClass;
using StatsAndTracking.Entity.Structure.Request;
using System.Collections.Generic;
using System.Linq;

namespace StatsAndTracking.Handler.CommandHandler.SetPD
{
    /// <summary>
    /// Set persist storage data
    /// </summary>
    internal class SetPDHandler : GStatsCommandHandlerBase
    {
        //@"\setpd\\pid\4\ptype\4\dindex\4\kv\\key1\value1\key2\value2\key3\value3\lid\2\length\5\data\final\"
        protected SetPDRequest _request;

        public SetPDHandler(ISession session, Dictionary<string, string> request) : base(session, request)
        {
            _request = new SetPDRequest(request);
        }

        protected override void CheckRequest()
        {
            _errorCode = _request.Parse();
        }

        protected override void DataOperation()
        {
            using (var db = new retrospyContext())
            {

                var result = from p in db.Pstorage
                             where p.Profileid == _request.ProfileID
                             && p.Dindex == _request.DataIndex
                             && p.Ptype == (uint)_request.StorageType
                             select p;

                Pstorage ps;
                if (result.Count() == 0)
                {
                    //insert a new record in database
                    ps = new Pstorage();
                    ps.Dindex = _request.DataIndex;
                    ps.Profileid = _request.ProfileID;
                    ps.Ptype = (uint)_request.StorageType;
                    ps.Data = _request.KeyValueString;
                    db.Pstorage.Add(ps);
                }
                else if (result.Count() == 1)
                {
                    //update an existed record in database
                    ps = result.First();
                    ps.Data = _request.KeyValueString;
                }

                db.SaveChanges();
            }
        }
    }
}
