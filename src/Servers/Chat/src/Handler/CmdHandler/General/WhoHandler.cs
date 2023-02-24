using System.Linq;
using UniSpy.Server.Chat.Abstraction.BaseClass;
using UniSpy.Server.Chat.Application;
using UniSpy.Server.Chat.Exception;
using UniSpy.Server.Chat.Exception.IRC.General;
using UniSpy.Server.Chat.Aggregate.Misc;
using UniSpy.Server.Chat.Contract.Request.General;
using UniSpy.Server.Chat.Contract.Response.General;
using UniSpy.Server.Chat.Contract.Result.General;
using UniSpy.Server.Chat.Handler.CmdHandler.Channel;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Chat.Aggregate;
using UniSpy.Server.Chat.Aggregate.Redis;
using UniSpy.Server.Chat.Abstraction.Interface;

namespace UniSpy.Server.Chat.Handler.CmdHandler.General
{
    /// <summary>
    /// Get a channel user's basic information
    /// same as WHOIS
    /// </summary>

    public sealed class WhoHandler : LogedInHandlerBase
    {
        private new WhoRequest _request => (WhoRequest)base._request;
        private new WhoResult _result { get => (WhoResult)base._result; set => base._result = value; }
        public WhoHandler(IClient client, IRequest request) : base(client, request) { }

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
                    UserName = user.Info.UserName,
                    NickName = user.Info.NickName,
                    PublicIPAddress = user.Connection.RemoteIPEndPoint.Address.ToString(),
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
            var localClient = (Client)Client.ClientPool.Values.FirstOrDefault(c => ((ClientInfo)(c.Info)).NickName == _request.NickName);
            var remoteClient = (RemoteClient)GeneralMessageChannel.RemoteClients.Values.FirstOrDefault(c => ((ClientInfo)(c.Info)).NickName == _request.NickName);
            IChatClient client;


            if (localClient is not null)
            {
                client = localClient;
            }
            else if (remoteClient is not null)
            {
                client = remoteClient;
            }
            else
            {
                throw new ChatIRCNoSuchNickException($"Can not find user with nickname:{_request.NickName}.");
            }


            foreach (var channel in ((ClientInfo)(client.Info)).JoinedChannels.Values)
            {
                var user = channel.GetChannelUser(client);
                var data = new WhoDataModel
                {
                    ChannelName = channel.Name,
                    NickName = client.Info.NickName,
                    UserName = client.Info.UserName,
                    PublicIPAddress = client.Connection.RemoteIPEndPoint.Address.ToString(),
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
