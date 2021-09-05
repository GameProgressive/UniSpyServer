using Chat.Abstraction.BaseClass;
using Chat.Entity.Exception.IRC.General;
using Chat.Entity.Structure.Request;
using Chat.Entity.Structure.Response.Channel;
using Chat.Entity.Structure.Result.Channel;
using UniSpyLib.Abstraction.Interface;

namespace Chat.Handler.CmdHandler.General
{
    /// <summary>
    /// Get value of the channel user's key value of all channels
    /// </summary>
    internal sealed class GETKEYHandler : ChatLogedInHandlerBase
    {
        private new GETKEYRequest _request => (GETKEYRequest)base._request;
        private new GETKEYResult _result
        {
            get => (GETKEYResult)base._result;
            set => base._result = value;
        }
        public GETKEYHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _result = new GETKEYResult();
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
            _response = new GETKEYResponse(_request, _result);
        }
    }
}
