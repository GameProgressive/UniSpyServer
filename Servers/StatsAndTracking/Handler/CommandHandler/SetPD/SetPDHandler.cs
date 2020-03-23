using GameSpyLib.Database.DatabaseModel.MySql;
using StatsAndTracking.Entity.Enumerator;
using System.Collections.Generic;
using System.Linq;

namespace StatsAndTracking.Handler.CommandHandler.SetPD
{
    /// <summary>
    /// Set persist storage data
    /// </summary>
    internal class SetPDHandler : CommandHandlerBase
    {
        //@"\setpd\\pid\4\ptype\4\dindex\4\kv\\key1\value1\key2\value2\key3\value3\lid\2\length\5\data\final\"
        private string _keyValueStr = "";
        private uint _profileid, _ptype, _dindex, _length;

        public SetPDHandler(GStatsSession session, Dictionary<string, string> recv) : base(session, recv)
        {
        }

        protected override void CheckRequest(GStatsSession session, Dictionary<string, string> recv)
        {
            base.CheckRequest(session, recv);

            if (!uint.TryParse(recv["pid"], out _profileid))
            {
                _errorCode = GstatsErrorCode.Parse;
                return;
            }

            if (!uint.TryParse(recv["ptype"], out _ptype))
            {
                _errorCode = GstatsErrorCode.Parse;
                return;
            }

            if (!uint.TryParse(recv["dindex"], out _dindex))
            {
                _errorCode = GstatsErrorCode.Parse;
                return;
            }

            if (!uint.TryParse(recv["length"], out _length))
            {
                _errorCode = GstatsErrorCode.Parse;
                return;
            }

            //we extract the key value data
            foreach (var d in recv.Skip(5))
            {
                if (d.Key.ToString() == "lid")
                    break;
                _keyValueStr += @"\" + d.Key + @"\" + d.Value;
            }

        }

        protected override void DataOperation(GStatsSession session, Dictionary<string, string> recv)
        {
            using (var db = new retrospyContext())
            {
                
                var result = from p in db.Pstorage
                             where p.Profileid == _profileid && p.Dindex == _dindex && p.Ptype == _ptype
                             select p;

                Pstorage ps;
                if (result.Count() == 0)
                {
                    //insert a new record in database
                    ps = new Pstorage();
                    ps.Dindex = _dindex;
                    ps.Profileid = _profileid;
                    ps.Ptype = _ptype;
                    ps.Data = _keyValueStr;
                    db.Pstorage.Add(ps);
                }
                else if (result.Count() == 1)
                {
                    //update an existed record in database
                    ps = result.First();
                    ps.Data = _keyValueStr;
                }
                
                db.SaveChanges();
            }
        }
    }
}
