using StatsAndTracking.Entity.Enumerator;
using System.Collections.Generic;

namespace StatsAndTracking.Handler.CommandHandler.GetPD
{
    public class GetPDHandler : CommandHandlerBase
    {
        //\getpd\\pid\%d\ptype\%d\dindex\%d\keys\%s\lid\%d
        private uint _profileid;
        private uint _persistantStorageType;
        private uint _dataIndex;

        public GetPDHandler(GStatsSession session, Dictionary<string, string> recv) : base(session, recv)
        {
        }

        protected override void CheckRequest(GStatsSession session, Dictionary<string, string> recv)
        {
            base.CheckRequest(session, recv);
            if (recv.ContainsKey("pid"))
            {
                if (!uint.TryParse(recv["pid"], out _profileid))
                {
                    _errorCode = GstatsErrorCode.Parse;
                    return;
                }
            }
            else
            {
                _errorCode = GstatsErrorCode.Parse;
                return;
            }
            if (recv.ContainsKey("ptype"))
            {
                if (!uint.TryParse(recv["ptype"], out _persistantStorageType))
                {
                    _errorCode = GstatsErrorCode.Parse;
                    return;
                }
            }
            else
            {
                _errorCode = GstatsErrorCode.Parse;
                return;
            }

            if (recv.ContainsKey("dindex"))
            {
                if (!uint.TryParse(recv["dindex"], out _dataIndex))
                {
                    _errorCode = GstatsErrorCode.Parse;
                    return;
                }
            }
            else
            {
                _errorCode = GstatsErrorCode.Parse;
                return;
            }
        }


        protected override void DataOperation(GStatsSession session, Dictionary<string, string> recv)
        {
            base.DataOperation(session, recv);
            //search player data in database;

            //using (var db = new retrospyContext())
            //{
            //    var result = from ps in db.Pstorage
            //                 where ps.Ptype == _persistantStorageType
            //                 && ps.Dindex == _dataIndex
            //                 && ps.Profileid == _profileid
            //                 select ps.Data;
            //    if (result.Count() != 1)
            //    {
            //        _errorCode = GstatsErrorCode.Database;
            //        return;
            //    }
            //    //TODO figure out what is the function of keys in request
            //    //throw new NotImplementedException();
            //}
        }
        protected override void ConstructResponse(GStatsSession session, Dictionary<string, string> recv)
        {
            _sendingBuffer = $@"\getpdr\1\pid\{recv["pid"]}\lid\{_localId}\mod\1234\length\5\data\mydata";
        }
    }
}
