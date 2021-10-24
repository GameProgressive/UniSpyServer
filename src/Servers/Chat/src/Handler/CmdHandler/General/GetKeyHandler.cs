using UniSpyServer.Chat.Abstraction.BaseClass;
using UniSpyServer.Chat.Entity.Contract;
using UniSpyServer.Chat.Entity.Exception.IRC.General;
using UniSpyServer.Chat.Entity.Structure.Request.General;
using UniSpyServer.Chat.Entity.Structure.Response.General;
using UniSpyServer.Chat.Entity.Structure.Result.General;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Chat.Handler.CmdHandler.General
{
    /// <summary>
    /// Get value of the channel user's key value of all channels
    /// </summary>
    [HandlerContract("GETKey")]
    public sealed class GetKeyHandler : LogedInHandlerBase
    {
        private new GetKeyRequest _request => (GetKeyRequest)base._request;
        private new GetKeyResult _result
        {
            get => (GetKeyResult)base._result;
            set => base._result = value;
        }
        public GetKeyHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _result = new GetKeyResult();
        }

        protected override void DataOperation()
        {
            _result.NickName = _session.UserInfo.NickName;
            foreach (var channel in _session.UserInfo.JoinedChannels)
            {
                var user = channel.GetChannelUserByNickName(_request.NickName);
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
