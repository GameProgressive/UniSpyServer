using System;
using Newtonsoft.Json;
using StackExchange.Redis;
using UniSpy.Server.Core.Config;

namespace UniSpy.Server.Core.Abstraction.BaseClass
{

    public abstract class RedisChannelBase<T> : IDisposable
    {
        public delegate void OnReceivedMessageEventHandler(T message);
        public event OnReceivedMessageEventHandler OnReceived;
        protected string _redisChannelName;
        /// <summary>
        /// Get all subscriber in Redis
        /// </summary>
        protected ISubscriber _subscriber = ConfigManager.Config.Redis.RedisConnection.GetSubscriber();
        public bool IsStarted { get; private set; }
        public RedisChannelBase(string redisChannelName)
        {
            _redisChannelName = redisChannelName;
            OnReceived += ReceivedMessage;
        }

        /// <summary>
        /// Run StartSubscribe manually
        /// Some server only require publish message
        /// so they do not need to run this function
        /// </summary>
        public void Subscribe()
        {
            if (IsStarted == false)
            {
                _subscriber.Subscribe(_redisChannelName, (channel, message)
                    =>
                {
                    T msg = DeserializeMessage(message);
                    OnReceived(msg);
                });
                IsStarted = true;
            }
        }

        public virtual void ReceivedMessage(T message)
        {

        }

        public virtual void PublishMessage(T message)
        {
            if (message is null)
            {
                return;
            }
            string jsonStr = SerializeMessage(message);
            _subscriber.Publish(_redisChannelName, jsonStr);
        }

        protected virtual string SerializeMessage(T message) => JsonConvert.SerializeObject(message);

        protected virtual T DeserializeMessage(string message) => JsonConvert.DeserializeObject<T>(message);
        public void Unsubscribe()
        {
            _subscriber.Unsubscribe(_redisChannelName);
            OnReceived -= ReceivedMessage;
        }
        public void Dispose() => Unsubscribe();
    }
}
