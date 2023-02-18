using Newtonsoft.Json;
using UniSpy.Server.Chat.Abstraction.BaseClass;
using UniSpy.Server.Chat.Application;

namespace UniSpy.Server.Chat.Aggregate.Redis.Contract
{
    public class ChannelMessage
    {
        [JsonIgnore]
        public string MessageType => Request.CommandName;
        public ChannelRequestBase Request { get; private set; }
        public Client Client { get; private set; }
        public ChannelMessage(ChannelRequestBase request, Client client)
        {
            Request = request;
            Client = client;
        }
    }
}