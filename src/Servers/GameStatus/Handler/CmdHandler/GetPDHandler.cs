using GameStatus.Abstraction.BaseClass;
using GameStatus.Entity.Enumerate;
using GameStatus.Entity.Structure.Request;
using GameStatus.Entity.Structure.Response;
using GameStatus.Entity.Structure.Result;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Database.DatabaseModel.MySql;

namespace GameStatus.Handler.CmdHandler
{
    internal sealed class GetPDHandler : GSCmdHandlerBase
    {
        //\getpd\\pid\%d\ptype\%d\dindex\%d\keys\%s\lid\%d
        private new GetPDRequest _request => (GetPDRequest)base._request;
        private new GetPDResult _result
        {
            get => (GetPDResult)base._result;
            set => base._result = value;
        }
        public GetPDHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _result = new GetPDResult();
        }
        protected override void DataOperation()
        {
            //search player data in database;
            Dictionary<string, string> keyValues;

            using (var db = new unispyContext())
            {
                var result = from ps in db.Pstorage
                             where ps.Ptype == (uint)_request.StorageType
                             && ps.Dindex == _request.DataIndex
                             && ps.Profileid == _request.ProfileID
                             select ps.Data;

                if (result.Count() != 1)
                {
                    _result.ErrorCode = GSErrorCode.Database;
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
                _result.KeyValues = keyValues;
            }
            else
            {
                foreach (var key in _request.Keys)
                {
                    if (keyValues.ContainsKey(key))
                    {
                        _result.KeyValues.Add(key, keyValues[key]);
                    }
                    else
                    {
                        _result.ErrorCode = GSErrorCode.Database;
                        break;
                    }
                }
            }
        }

        protected override void ResponseConstruct()
        {
            _response = new GetPDResponse(_request, _result);
        }
    }
}
