using System;
using System.Linq;
using UniSpy.Server.Chat.Abstraction.BaseClass;
using UniSpy.Server.Chat.Application;
using UniSpy.Server.Chat.Error.IRC.General;
using UniSpy.Server.Chat.Contract.Request.General;
using UniSpy.Server.Chat.Contract.Response.General;
using UniSpy.Server.Chat.Abstraction.Interface;
using UniSpy.Server.Chat.Contract.Result.General;
using UniSpy.Server.Chat.Aggregate.Redis;

namespace UniSpy.Server.Chat.Handler.CmdHandler.General
{

    public sealed class NickHandler : CmdHandlerBase
    {
        private new NickRequest _request => (NickRequest)base._request;
        private new NickResult _result { get => (NickResult)base._result; set => base._result = value; }
        private string _tempNickName;
        public NickHandler(IShareClient client, NickRequest request) : base(client, request)
        {
            _result = new NickResult();
        }


        private void SetUniqueNickAsNickName()
        {
            if (_client.Info.UniqueNickName is null)
            {
                throw new Chat.Exception("uniquenick is not set.");
            }
            if (_client.Info.UniqueNickName == "")
            {
                throw new Chat.Exception("uniquenick can not be empty string");
            }
            var postFix = "";
            if (_client.Info.GameName.Length > 2)
            {
                postFix = _client.Info.GameName.Substring(0, 3);
            }
            else
            {
                postFix = _client.Info.GameName.Substring(0, 2);
            }
            _tempNickName = $"{_client.Info.UniqueNickName}-{postFix}";
        }
        protected override void DataOperation()
        {
            if (_request.NickName == "*")
            {
                //client using its <uniquenick>-<gamename> as his nickname in chat
                SetUniqueNickAsNickName();
            }
            else
            {
                _tempNickName = _request.NickName;
            }
            var key = new ClientInfoCache { NickName = _tempNickName };
            using (var locker = new LinqToRedis.RedisLock(TimeSpan.FromSeconds(10), Application.StorageOperation.Persistance.ClientCacheClient.Db, key))
            {
                if (locker.LockTake())
                {
                    if (!Application.StorageOperation.Persistance.IsClientExist(key))
                    {
                        _client.Info.NickName = _request.NickName;
                        Application.StorageOperation.Persistance.UpdateClient((Client)_client);
                    }
                    else
                    {
                        throw new NickNameInUseException();
                    }
                }
                else
                {
                    throw new NickNameInUseException();
                }

            }

            _result.NickName = _client.Info.NickName;
        }
        protected override void ResponseConstruct()
        {
            _response = new NickResponse(_request, _result);
        }
    }
}
