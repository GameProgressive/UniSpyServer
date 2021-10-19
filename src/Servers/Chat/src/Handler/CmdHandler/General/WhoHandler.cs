using Chat.Abstraction.BaseClass;
using Chat.Application;
using Chat.Entity.Contract;
using Chat.Entity.Exception;
using Chat.Entity.Exception.IRC.General;
using Chat.Entity.Structure.Misc;
using Chat.Entity.Structure.Misc.ChannelInfo;
using Chat.Entity.Structure.Request.General;
using Chat.Entity.Structure.Response.General;
using Chat.Entity.Structure.Result.General;
using Chat.Handler.SystemHandler.ChannelManage;
using Chat.Network;
using System.Linq;
using UniSpyLib.Abstraction.Interface;
namespace Chat.Handler.CmdHandler.General
{
    /// <summary>
    /// Get a channel user's basic information
    /// same as WHOIS
    /// </summary>
    [HandlerContract("WHO")]
    public sealed class WhoHandler : LogedInHandlerBase
    {
        private new WhoRequest _request => (WhoRequest)base._request;
        private new WhoResult _result
        {
            get => (WhoResult)base._result;
            set => base._result = value;
        }
        public WhoHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _result = new WhoResult();
        }

        protected override void DataOperation()
        {
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
            Entity.Structure.Misc.ChannelInfo.Channel channel;
            if (!ChatChannelManager.GetChannel(_request.ChannelName, out channel))
            {
                throw new IRCChannelException($"The channel is not exist.", IRCErrorCode.NoSuchChannel, _request.ChannelName);
            }
            foreach (var user in channel.Property.ChannelUsers)
            {
                var data = new WhoDataModel
                {
                    ChannelName = channel.Property.ChannelName,
                    UserName = user.UserInfo.UserName,
                    NickName = user.UserInfo.NickName,
                    PublicIPAddress = user.UserInfo.Session.RemoteIPEndPoint.Address.ToString(),
                    Modes = user.GetUserModes()
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
            foreach (var channel in session.UserInfo.JoinedChannels)
            {
                ChannelUser user = channel.GetChannelUserBySession(session);
                var data = new WhoDataModel
                {
                    ChannelName = channel.Property.ChannelName,
                    NickName = session.UserInfo.NickName,
                    UserName = session.UserInfo.UserName,
                    PublicIPAddress = session.RemoteIPEndPoint.Address.ToString(),
                    Modes = user.GetUserModes()
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
