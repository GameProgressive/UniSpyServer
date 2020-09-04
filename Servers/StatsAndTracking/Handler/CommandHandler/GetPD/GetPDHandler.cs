using GameSpyLib.Common.Entity.Interface;
using StatsAndTracking.Entity.Structure.Request;
using System.Collections.Generic;

namespace StatsAndTracking.Handler.CommandHandler.GetPD
{
    public class GetPDHandler : GStatsCommandHandlerBase
    {
        //\getpd\\pid\%d\ptype\%d\dindex\%d\keys\%s\lid\%d
        protected GetPDRequest _request;

        public GetPDHandler(ISession session, Dictionary<string, string> request) : base(session, request)
        {
            _request = new GetPDRequest(request);
        }

        protected override void CheckRequest()
        {
            _errorCode = _request.Parse();
        }

        protected override void DataOperation()
        {
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
        protected override void ConstructResponse()
        {
            _sendingBuffer = $@"\getpdr\1\pid\{_request.ProfileID}\lid\{_request.OperationID}\mod\1234\length\5\data\mydata";
        }
    }
}
