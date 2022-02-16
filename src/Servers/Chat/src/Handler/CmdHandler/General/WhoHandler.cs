using System.Linq;
using UniSpyServer.Servers.Chat.Abstraction.BaseClass;
using UniSpyServer.Servers.Chat.Application;
using UniSpyServer.Servers.Chat.Entity.Contract;
using UniSpyServer.Servers.Chat.Entity.Exception;
using UniSpyServer.Servers.Chat.Entity.Exception.IRC.General;
using UniSpyServer.Servers.Chat.Entity.Structure.Misc;
using UniSpyServer.Servers.Chat.Entity.Structure.Request.General;
using UniSpyServer.Servers.Chat.Entity.Structure.Response.General;
using UniSpyServer.Servers.Chat.Entity.Structure.Result.General;
using UniSpyServer.Servers.Chat.Handler.CmdHandler.Channel;
using UniSpyServer.Servers.Chat.Network;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.Chat.Handler.CmdHandler.General
{
    /// <summary>
    /// Get a channel user's basic information
    /// same as WHOIS
    /// </summary>
    [HandlerContract("WHO")]
    public sealed class WhoHandler : LogedInHandlerBase
    {
        private new WhoRequest _request => (WhoRequest)base._request;
        private new WhoResult _result { get => (WhoResult)base._result; set => base._result = value; }
        public WhoHandler(IClient client, IRequest request) : base(client, request)
        {
        }

        protected override void DataOperation()
        {
            _result = new WhoResult();

            switch (_request.RequestType)
            {
                case WhoRequestType.GetChannelUsersInfo:
                    GetChannelUsersInfo();
                    break;
                case WhoRequestType.GetUserInfo:
                    GetUserInfo();
                    break;
            }
        }

        private void GetChannelUsersInfo()
        {
            if (!JoinHandler.Channels.ContainsKey(_request.ChannelName))
            {
                throw new IRCChannelException($"The channel is not exist.", IRCErrorCode.NoSuchChannel, _request.ChannelName);
            }
            var channel = JoinHandler.Channels[_request.ChannelName];
            foreach (var user in channel.Users.Values)
            {
                var data = new WhoDataModel
                {
                    ChannelName = channel.Name,
                    UserName = user.UserInfo.UserName,
                    NickName = user.UserInfo.NickName,
                    PublicIPAddress = user.UserInfo.Session.RemoteIPEndPoint.Address.ToString(),
                    Modes = user.Modes
                };
                _result.DataModels.Add(data);
            }
        }
        /// <summary>
        /// Send all channel user information
        /// </summary>
        private void GetUserInfo()
        {
            var session = (Session)ServerFactory.Server.SessionManager.SessionPool.Values
                .Where(s => ((Session)s).UserInfo.NickName == _request.NickName)
                .FirstOrDefault();
            if (session == null)
            {
                throw new ChatIRCNoSuchNickException($"Can not find user with nickname:{_request.NickName}.");

            }
            foreach (var channel in session.UserInfo.JoinedChannels.Values)
            {
                var user = channel.GetChannelUserBySession(session);
                var data = new WhoDataModel
                {
                    ChannelName = channel.Name,
                    NickName = session.UserInfo.NickName,
                    UserName = session.UserInfo.UserName,
                    PublicIPAddress = session.RemoteIPEndPoint.Address.ToString(),
                    Modes = user.Modes
                };
                _result.DataModels.Add(data);
            }
        }
        protected override void ResponseConstruct()
        {
            _response = new WhoResponse(_request, _result);
        }
    }
}
