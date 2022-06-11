using UniSpyServer.Servers.Chat.Abstraction.BaseClass;

using UniSpyServer.Servers.Chat.Entity.Exception.IRC.General;
using UniSpyServer.Servers.Chat.Entity.Structure.Request.General;
using UniSpyServer.Servers.Chat.Entity.Structure.Response.General;
using UniSpyServer.Servers.Chat.Entity.Structure.Result.General;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.Chat.Handler.CmdHandler.General
{
    /// <summary>
    /// Get value of the channel user's key value of all channels
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
            _result.NickName = _client.Info.NickName;
            foreach (var channel in _client.Info.JoinedChannels.Values)
            {
                var user = channel.GetChannelUser(_request.NickName);
                if (user == null)
                {
                    throw new ChatIRCNoSuchNickException($"Can not find user:{_request.NickName}");
                }
                else
                {
                    var values = user.GetUserValues(_request.Keys);
                    _result.Flags.Add(values);
                }
            }
        }

        protected override void ResponseConstruct()
        {
            _response = new GetKeyResponse(_request, _result);
        }
    }
}
