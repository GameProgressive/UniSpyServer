using GameSpyLib.Database.DatabaseModel.MySql;
using StatsAndTracking.Entity.Enumerator;
using System.Collections.Generic;
using System.Linq;

namespace StatsAndTracking.Handler.CommandHandler.SetPD
{
    internal class SetPDHandler : GStatsHandlerBase
    {
        //@"\setpd\\pid\4\ptype\4\dindex\4\kv\\key1\value1\key2\value2\key3\value3\lid\2\length\5\data\final\"
        private string _keyValueStr = "";
        private uint _profileid, _ptype, _dindex, _length;

        protected SetPDHandler(GStatsSession session, Dictionary<string, string> recv) : base(session, recv)
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
                db.Pstorage.Where(p => p.Profileid == _profileid && p.Dindex == _dindex).FirstOrDefault().Data = _keyValueStr;
                db.SaveChanges();
            }
        }
    }
}
