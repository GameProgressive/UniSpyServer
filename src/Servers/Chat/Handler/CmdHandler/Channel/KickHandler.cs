using Chat.Abstraction.BaseClass;
using Chat.Entity.Contract;
using Chat.Entity.Exception;
using Chat.Entity.Structure.Misc.ChannelInfo;
using Chat.Entity.Structure.Request;
using Chat.Entity.Structure.Response.Channel;
using Chat.Entity.Structure.Result.Channel;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Extensions;

namespace Chat.Handler.CmdHandler.Channel
{
    [HandlerContract("KICK")]
    internal sealed class KickHandler : ChannelHandlerBase
    {
        private new KickRequest _request => (KickRequest)base._request;
        private new KICKResponse _response
        {
            get => (KICKResponse)base._response;
            set => base._response = value;
        }
        private new KICKResult _result
        {
            get => (KICKResult)base._result;
            set => base._result = value;
        }
        private ChatChannelUser _kickee;
        public KickHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _result = new KICKResult();
        }

        protected override void RequestCheck()
        {
            base.RequestCheck();

            if (!_user.IsChannelOperator)
            {
                throw new ChatException("Kick operation failed because you are not channel operator.");
            }
            _kickee = _channel.GetChannelUserByNickName(_request.KickeeNickName);
            if (_kickee != null)
            {
                throw new ChatException($"Can not find kickee:{_request.KickeeNickName} in channel.");
            }
        }
        protected override void DataOperation()
        {
            _channel.RemoveBindOnUserAndChannel(_kickee);
            _result.KickeeNickName = _session.UserInfo.NickName;
            _result.KickerIRCPrefix = _session.UserInfo.IRCPrefix;
            _result.KickeeNickName = _kickee.UserInfo.NickName;
        }

        protected override void ResponseConstruct()
        {
            _response = new KICKResponse(_request, _result);
        }

        protected override void Response()
        {
            _response.Build();
            if (!StringExtensions.CheckResponseValidation(_response.SendingBuffer))
            {
                return;
            }
            _channel.MultiCast(_response.SendingBuffer);
        }
    }
}
