using System.Linq;
using UniSpy.Server.Chat.Abstraction.BaseClass;
using UniSpy.Server.Chat.Application;
using UniSpy.Server.Chat.Error.IRC.General;
using UniSpy.Server.Chat.Contract.Request.General;
using UniSpy.Server.Chat.Contract.Response.General;
using UniSpy.Server.Chat.Abstraction.Interface;
using UniSpy.Server.Chat.Contract.Result.General;

namespace UniSpy.Server.Chat.Handler.CmdHandler.General
{

    public sealed class NickHandler : CmdHandlerBase
    {
        private new NickRequest _request => (NickRequest)base._request;
        private new NickResult _result { get => (NickResult)base._result; set => base._result = value; }
        public NickHandler(IChatClient client, NickRequest request) : base(client, request)
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
            _client.Info.NickName = $"{_client.Info.UniqueNickName}-{postFix}";
        }
        private void SuggestNewNickName()
        {
            int number = 0;
            string validNickName;
            while (true)
            {
                string newNickName = _request.NickName + number;
                if (ClientManager.ClientPool.Values.Count(c => ((ClientInfo)(c.Info)).NickName == _request.NickName) == 0)
                {
                    validNickName = newNickName;
                    break;
                }
            }
            throw new NickNameInUseException(
            $"The nick name: {_request.NickName} is already in use",
            _request.NickName,
            validNickName);
        }
        protected override void DataOperation()
        {
            if (_request.NickName == "*")
            {
                //client using its <uniquenick>-<gamename> as his nickname in chat
                SetUniqueNickAsNickName();
            }
            else if (ClientManager.ClientPool.Values.Count(c => ((ClientInfo)(c.Info)).NickName == _request.NickName) == 0)
            {
                _client.Info.NickName = _request.NickName;
            }
            else
            {
                SuggestNewNickName();
            }
            _result.NickName = _client.Info.NickName;
        }
        protected override void ResponseConstruct()
        {
            _response = new NickResponse(_request, _result);
        }
    }
}
