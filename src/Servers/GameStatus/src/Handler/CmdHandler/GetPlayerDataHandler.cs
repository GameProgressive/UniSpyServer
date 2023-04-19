using System.Collections.Generic;
using UniSpy.Server.GameStatus.Abstraction.BaseClass;
using UniSpy.Server.GameStatus.Application;

using UniSpy.Server.GameStatus.Contract.Request;
using UniSpy.Server.GameStatus.Contract.Response;
using UniSpy.Server.GameStatus.Contract.Result;
using UniSpy.Server.Core.Abstraction.Interface;

namespace UniSpy.Server.GameStatus.Handler.CmdHandler
{

    public sealed class GetPlayerDataHandler : CmdHandlerBase
    {
        //\getpd\\pid\%d\ptype\%d\dindex\%d\keys\%s\lid\%d
        private new GetPlayerDataRequest _request => (GetPlayerDataRequest)base._request;
        private new GetPlayerDataResult _result { get => (GetPlayerDataResult)base._result; set => base._result = value; }
        public GetPlayerDataHandler(Client client, GetPlayerDataRequest request) : base(client, request)
        {
            _result = new GetPlayerDataResult();
        }
        protected override void DataOperation()
        {
            //search player data in database;
            Dictionary<string, string> keyValues;

            keyValues = StorageOperation.Persistance.GetPlayerData(_request.ProfileId, _request.StorageType, _request.DataIndex);
            //TODO figure out what is the function of keys in request


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
                        throw new GameStatus.Exception($"can not find key:{key} in GetPD request.");
                    }
                }
            }
        }

        protected override void ResponseConstruct()
        {
            _response = new GetPlayerDataResponse(_request, _result);
        }
    }
}
