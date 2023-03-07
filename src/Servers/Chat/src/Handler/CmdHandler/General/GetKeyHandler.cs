using UniSpy.Server.Chat.Abstraction.BaseClass;
using UniSpy.Server.Chat.Contract.Request.General;
using UniSpy.Server.Chat.Contract.Response.General;
using UniSpy.Server.Chat.Contract.Result.General;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Chat.Application;
using System.Linq;
using UniSpy.Server.Chat.Aggregate.Redis;

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
                var matchedClients = Client.ClientPool.Values.Select(c => c.Info).ToList();
                matchedClients.AddRange(GeneralMessageChannel.RemoteClients.Values.Select(c => c.Info).ToList());
                foreach (ClientInfo info in matchedClients)
                {
                    var values = info.GetUserValues(_request.Keys);
                    _result.Values.Add(values);
                }
            }
            else
            {
                _result.NickName = _request.NickName;
                var target = Client.ClientPool.Values.Where(c => ((ClientInfo)(c.Info)).NickName == _request.NickName).First();
                _result.Values.Add(_client.Info.GetUserValues(_request.Keys));
            }
        }

        protected override void ResponseConstruct()
        {
            _response = new GetKeyResponse(_request, _result);
        }
    }
}
