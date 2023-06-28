using Newtonsoft.Json;
using UniSpy.Server.Chat.Abstraction.Interface;
using UniSpy.Server.Chat.Aggregate.Misc;
using UniSpy.Server.Chat.Application;
using UniSpy.Server.Chat.Handler;
using UniSpy.Server.Core.Abstraction.BaseClass;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Core.Encryption;
using UniSpy.Server.Core.Logging;
using UniSpy.Server.Core.Network;

namespace UniSpy.Server.Chat.Aggregate
{
    public class RemoteClient : IShareClient
    {
        public bool IsLogRaw { get; set; }
        [JsonConverter(typeof(ConcreteTypeConverter<RemoteTcpConnection>))]
        public IConnection Connection { get; set; }
        [JsonConverter(typeof(ConcreteTypeConverter<ChatCrypt>))]
        public ICryptography Crypto { get; set; }
        [JsonConverter(typeof(ConcreteTypeConverter<ClientInfo>))]
        public ClientInfoBase Info { get; set; }
        [JsonIgnore]
        ClientInfo IShareClient.Info => (ClientInfo)Info;
        [JsonConverter(typeof(ConcreteTypeConverter<RemoteServer>))]
        public IServer Server { get; set; }
        /// <summary>
        /// using for json deserialization
        /// </summary>
        public RemoteClient() { }
        public RemoteClient(Client client)
        {
            Connection = new RemoteTcpConnection(client.Connection, new RemoteTcpConnectionManager());
            Server = new RemoteServer(client.Server);
            Info = client.Info.DeepCopy();
            ((ClientInfo)Info).IsRemoteClient = true;
            Crypto = client.Crypto;
        }
        public void Send(IResponse response) { }
        public RemoteClient GetRemoteClient() => this;

        public void TestReceived(byte[] buffer)
        {
            this.LogNetworkReceiving(buffer);
            var switcher = new CmdSwitcher(this, UniSpyEncoding.GetString(buffer));
            switcher.Handle();
        }
    }
}