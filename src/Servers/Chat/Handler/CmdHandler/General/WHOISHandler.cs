using Chat.Abstraction.BaseClass;
using Chat.Application;
using Chat.Entity.Exception;
using Chat.Entity.Exception.IRC.General;
using Chat.Entity.Structure;
using Chat.Entity.Structure.Misc;
using Chat.Entity.Structure.Request.General;
using Chat.Entity.Structure.Response.General;
using Chat.Entity.Structure.Result.General;
using Chat.Network;
using System.Linq;
using UniSpyLib.Abstraction.Interface;

namespace Chat.Handler.CmdHandler.General
{
    internal sealed class WHOISHandler : ChatCmdHandlerBase
    {
        private new WHOISRequest _request => (WHOISRequest)base._request;
        private new WHOISResult _result
        {
            get => (WHOISResult)base._result;
            set => base._result = value;
        }
        private ChatUserInfo _userInfo;
        public WHOISHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _result = new WHOISResult();
        }

        protected override void RequestCheck()
        {
            // there only existed one nick name
            var session = (ChatSession)ChatServerFactory.Server.SessionManager.SessionPool.Values
                 .Where(s => ((ChatSession)s).UserInfo.NickName == _request.NickName)
                 .FirstOrDefault();
            if (session == null)
            {
                throw new ChatIRCNoSuchNickException($"Can not find user with nickname:{_request.NickName}.");
            }
            _userInfo = session.UserInfo;
        }
        protected override void DataOperation()
        {
            _result.NickName = _userInfo.NickName;
            _result.Name = _userInfo.Name;
            _result.UserName = _userInfo.UserName;
            _result.PublicIPAddress = _userInfo.PublicIPAddress;
            foreach (var channel in _userInfo.JoinedChannels)
            {
                _result.JoinedChannelName.Add(channel.Property.ChannelName);
            }
        }
        protected override void ResponseConstruct()
        {
            _response = new WHOISResponse(_request, _result);
        }


    }
}
