using UniSpy.Server.Chat.Error.IRC.Channel;
using UniSpy.Server.Chat.Error.IRC.General;
using UniSpy.Server.Chat.Aggregate;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Chat.Abstraction.Interface;
using UniSpy.Server.Chat.Aggregate.Redis.Contract;
using System;
using UniSpy.Server.Chat.Aggregate.Redis;

namespace UniSpy.Server.Chat.Abstraction.BaseClass
{
    public abstract class ChannelHandlerBase : LogedInHandlerBase
    {
        /// <summary>
        /// The matched channel of chat request
        /// </summary>
        protected Channel _channel;
        /// <summary>
        /// The channel user of current IClient
        /// </summary>
        protected ChannelUser _user;
        private new ChannelRequestBase _request => (ChannelRequestBase)base._request;
        public ChannelHandlerBase(IShareClient client, IRequest request) : base(client, request) { }
        protected override void RequestCheck()
        {
            if (_request.RawRequest is not null)
            {
                base.RequestCheck();
            }
            if (_channel is null)
            {
                _channel = _client.Info.GetLocalJoinedChannel(_request.ChannelName);
            }
            if (_channel is null)
            {
                throw new NoSuchChannelException($"No such channel {_request.ChannelName}", _request.ChannelName);
            }
            if (_user is null)
            {
                _user = _channel.GetUser(_client);
            }
            if (_user is null)
            {
                throw new NoSuchNickException($"Can not find user with nickname: {_client.Info.NickName} username: {_client.Info.UserName}");
            }
        }

        public override void Handle()
        {
            base.Handle();
            try
            {
                PublishMessage();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
        /// <summary>
        /// publish message to redis channel, only localclient can publish message
        /// </summary>
        protected virtual void PublishMessage()
        {
            // we do not publish message when the message is received from remote client
            if (_client.IsRemoteClient)
            {
                return;
            }
            if (_channel is null)
            {
                return;
            }
            if (_request.RawRequest is null)
            {
                return;
            }

            var key = new ChannelCache
            {
                ChannelName = _channel.Name,
                GameName = _channel.GameName
            };
            using (var locker = new LinqToRedis.RedisLock(TimeSpan.FromSeconds(10), Application.StorageOperation.Persistance.ChannelCacheClient.Db, key))
            {
                if (locker.LockTake())
                {
                    Aggregate.Channel.UpdateChannelCache(_user, _channel);
                }
            }

            var msg = new RemoteMessage(_request, _client.GetRemoteClient());
            _channel.Broker.PublishMessage(msg);
        }
    }
}
