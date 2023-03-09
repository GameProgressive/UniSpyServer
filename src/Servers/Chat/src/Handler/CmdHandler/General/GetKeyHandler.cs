using UniSpy.Server.Chat.Abstraction.BaseClass;
using UniSpy.Server.Chat.Contract.Request.General;
using UniSpy.Server.Chat.Contract.Response.General;
using UniSpy.Server.Chat.Contract.Result.General;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Chat.Application;


namespace UniSpy.Server.Chat.Handler.CmdHandler.General
{
    /// <summary>
    /// Get key value of the one user or all user
    /// </summary>
    public sealed class GetKeyHandler : LogedInHandlerBase
    {
        private new GetKeyRequest _request => (GetKeyRequest)base._request;
        private new GetKeyResult _result { get => (GetKeyResult)base._result; set => base._result = value; }
        public GetKeyHandler(IClient client, IRequest request) : base(client, request)
        {
            _result = new GetKeyResult();
        }

        protected override void DataOperation()
        {
            if (_request.IsGetAllUser)
            {
                var matchedClients = ClientManager.GetAllClientInfo();
                foreach (ClientInfo info in matchedClients)
                {
                    var values = info.KeyValues.GetValueString(_request.Keys);
                    _result.Values.Add(values);
                }
            }
            else
            {
                _result.NickName = _request.NickName;
                var target = ClientManager.GetClientByNickName(_request.NickName);
                _result.Values.Add(_client.Info.KeyValues.GetValueString(_request.Keys));
            }
        }

        protected override void ResponseConstruct()
        {
            _response = new GetKeyResponse(_request, _result);
        }
    }
}
