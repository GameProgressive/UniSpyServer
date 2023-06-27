using System.Net;
using System;
using UniSpy.Server.PresenceConnectionManager.Application;
using UniSpy.Server.Core.Misc;
using Newtonsoft.Json;

namespace UniSpy.Server.PresenceConnectionManager.Aggregate.Redis
{
    public class UserInfoMessage
    {
        [JsonProperty]
        public RemoteClient Client { get; private set; }
        public UserInfoMessage() { }
        public UserInfoMessage(RemoteClient client)
        {
            Client = client;
        }
    }
}