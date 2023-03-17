using Newtonsoft.Json;
using UniSpy.Server.Chat.Abstraction.BaseClass;
using UniSpy.Server.Core.Encryption;

namespace UniSpy.Server.Chat.Aggregate.Redis.Contract
{
    public class RemoteMessage
    {
        [JsonProperty]
        public string Type { get; private set; }
        [JsonProperty]
        public byte[] RawRequest { get; private set; }
        [JsonProperty]
        public RemoteClient Client { get; private set; }
        /// <summary>
        /// Constructor for json deserialization
        /// </summary>
        public RemoteMessage(){}
        public RemoteMessage(RequestBase request, RemoteClient client)
        {
            RawRequest = UniSpyEncoding.GetBytes(request.RawRequest);
            Type = request.CommandName;
            Client = client;
        }
    }
}