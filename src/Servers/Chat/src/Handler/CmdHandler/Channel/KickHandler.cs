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
        private new KickResponse _response
        {
            get => (KickResponse)base._response;
            set => base._response = value;
        }
        private new KickResult _result
        {
            get => (KickResult)base._result;
            set => base._result = value;
        }
        private ChannelUser _kickee;
        public KickHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _result = new KickResult();
        }

        protected override void RequestCheck()
        {
            base.RequestCheck();

            if (!_user.IsChannelOperator)
            {
                throw new Exception("Kick operation failed because you are not channel operator.");
            }
            _kickee = _channel.GetChannelUserByNickName(_request.KickeeNickName);
            if (_kickee != null)
            {
                throw new Exception($"Can not find kickee:{_request.KickeeNickName} in channel.");
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
            _response = new KickResponse(_request, _result);
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
