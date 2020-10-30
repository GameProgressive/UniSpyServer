﻿using GameSpyLib.Abstraction.Interface;
using GameSpyLib.Database.DatabaseModel.MySql;
using Newtonsoft.Json;
using StatsAndTracking.Abstraction.BaseClass;
using StatsAndTracking.Entity.Enumerate;
using StatsAndTracking.Entity.Structure.Request;
using System.Collections.Generic;
using System.Linq;

namespace StatsAndTracking.Handler.CommandHandler.GetPD
{
    public class GetPDHandler : GStatsCommandHandlerBase
    {
        //\getpd\\pid\%d\ptype\%d\dindex\%d\keys\%s\lid\%d
        protected GetPDRequest _request;
        protected Dictionary<string, string> _result;
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
            Dictionary<string, string> keyValues;

            using (var db = new retrospyContext())
            {
                var result = from ps in db.Pstorage
                             where ps.Ptype == (uint)_request.StorageType
                             && ps.Dindex == _request.DataIndex
                             && ps.Profileid == _request.ProfileID
                             select ps.Data;

                if (result.Count() != 1)
                {
                    _errorCode = GStatsErrorCode.Database;
                    return;
                }
                else
                {
                    keyValues = JsonConvert.DeserializeObject<Dictionary<string, string>>(result.First());
                }
                //TODO figure out what is the function of keys in request
            }

            if (_request.GetAllDataFlag)
            {
                _result = keyValues;
            }
            else
            {
                foreach (var key in _request.Keys)
                {
                    if (keyValues.ContainsKey(key))
                    {
                        _result.Add(key, keyValues[key]);
                    }
                    else
                    {
                        _errorCode = GStatsErrorCode.Database;
                        break;
                    }
                }
            }
        }

        protected override void ConstructResponse()
        {
            _sendingBuffer = $@"\getpdr\1\pid\{_request.ProfileID}\lid\{_request.OperationID}\mod\1234\length\5\data\mydata";
        }
    }
}
