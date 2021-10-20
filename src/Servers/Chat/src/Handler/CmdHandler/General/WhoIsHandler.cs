using UniSpyServer.Chat.Abstraction.BaseClass;
using UniSpyServer.Chat.Application;
using UniSpyServer.Chat.Entity.Contract;
using UniSpyServer.Chat.Entity.Exception.IRC.General;
using UniSpyServer.Chat.Entity.Structure.Misc;
using UniSpyServer.Chat.Entity.Structure.Request.General;
using UniSpyServer.Chat.Entity.Structure.Response.General;
using UniSpyServer.Chat.Entity.Structure.Result.General;
using UniSpyServer.Chat.Network;
using System.Linq;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Chat.Handler.CmdHandler.General
{
    [HandlerContract("WHOIS")]
    public sealed class WhoIsHandler : CmdHandlerBase
    {
        private new WhoIsRequest _request => (WhoIsRequest)base._request;
        private new WhoIsResult _result
        {
            get => (WhoIsResult)base._result;
            set => base._result = value;
        }
        private UserInfo _userInfo;
        public WhoIsHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _result = new WhoIsResult();
        }

        protected override void RequestCheck()
        {
            // there only existed one nick name
            base.RequestCheck();
            var session = (Session)ServerFactory.Server.SessionManager.SessionPool.Values
                 .Where(s => ((Session)s).UserInfo.NickName == _request.NickName)
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
            _response = new WhoIsResponse(_request, _result);
        }


    }
}
