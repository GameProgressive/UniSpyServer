using Chat.Abstraction.BaseClass;
using Chat.Application;
using Chat.Entity.Exception;
using Chat.Entity.Exception.IRC.General;
using Chat.Entity.Structure;
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
    internal sealed class WHOHandler : ChatLogedInHandlerBase
    {
        private new WHORequest _request => (WHORequest)base._request;
        private new WHOResult _result
        {
            get => (WHOResult)base._result;
            set => base._result = value;
        }
        public WHOHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _result = new WHOResult();
        }

        protected override void DataOperation()
        {
            switch (_request.RequestType)
            {
                case WHOType.GetChannelUsersInfo:
                    GetChannelUsersInfo();
                    break;
                case WHOType.GetUserInfo:
                    GetUserInfo();
                    break;
            }
        }

        private void GetChannelUsersInfo()
        {
            ChatChannel channel;
            if (!ChatChannelManager.GetChannel(_request.ChannelName, out channel))
            {
                throw new ChatIRCChannelException($"The channel is not exist.", ChatIRCErrorCode.NoSuchChannel, _request.ChannelName);
            }
            foreach (var user in channel.Property.ChannelUsers)
            {
                var data = new WHODataModel
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
            var session = (ChatSession)ChatServerFactory.Server.SessionManager.SessionPool.Values
                .Where(s => ((ChatSession)s).UserInfo.NickName == _request.NickName)
                .FirstOrDefault();
            if (session == null)
            {
                throw new ChatIRCNoSuchNickException($"Can not find user with nickname:{_request.NickName}.");

            }
            foreach (var channel in session.UserInfo.JoinedChannels)
            {
                ChatChannelUser user = channel.GetChannelUserBySession(session);
                var data = new WHODataModel
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
            _response = new WHOResponse(_request, _result);
        }
    }
}
