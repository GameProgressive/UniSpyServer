using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Database.DatabaseModel.MySql;
using Newtonsoft.Json;
using GameStatus.Abstraction.BaseClass;
using GameStatus.Entity.Enumerate;
using GameStatus.Entity.Structure.Request;
using System.Collections.Generic;
using System.Linq;

namespace GameStatus.Handler.CmdHandler
{
    public class GetPDHandler : GSCmdHandlerBase
    {
        //\getpd\\pid\%d\ptype\%d\dindex\%d\keys\%s\lid\%d
        protected new GetPDRequest _request { get { return (GetPDRequest)base._request; } }
        protected Dictionary<string, string> _result;
        public GetPDHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
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
                    _errorCode = GSError.Database;
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
                        _errorCode = GSError.Database;
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
