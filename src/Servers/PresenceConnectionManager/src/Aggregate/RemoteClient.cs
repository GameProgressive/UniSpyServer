using Newtonsoft.Json;
using UniSpy.Server.Core.Abstraction.BaseClass;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Core.Encryption;
using UniSpy.Server.Core.Logging;
using UniSpy.Server.Core.Network;
using UniSpy.Server.PresenceConnectionManager.Abstraction.Interface;
using UniSpy.Server.PresenceConnectionManager.Application;
using UniSpy.Server.PresenceConnectionManager.Handler;

namespace UniSpy.Server.PresenceConnectionManager.Aggregate
{

    public class RemoteClient : IShareClient
    {
        public bool IsLogRaw { get; set; }
        [JsonConverter(typeof(ConcreteTypeConverter<RemoteTcpConnection>))]
        public IConnection Connection { get; set; }
        public ICryptography Crypto => null;
        [JsonConverter(typeof(ConcreteTypeConverter<ClientInfo>))]
        public ClientInfoBase Info { get; set; }
        ClientInfo IShareClient.Info => (ClientInfo)Info;
        [JsonConverter(typeof(ConcreteTypeConverter<RemoteServer>))]
        public IServer Server { get; set; }
        /// <summary>
        /// using for json deserialization
        /// </summary>
        /// public RemoteClient() { }
        public RemoteClient(Client client)
        {
            Connection = new RemoteTcpConnection(client.Connection, new RemoteTcpConnectionManager());
            Server = new RemoteServer(client.Server);
            Info = client.Info.DeepCopy();
            ((ClientInfo)Info).IsRemoteClient = true;
        }

        public RemoteClient GetRemoteClient() => this;

        public void Send(IResponse response) { }
        public void TestReceived(byte[] buffer)
        {
            this.LogNetworkReceiving(buffer);
            var switcher = new CmdSwitcher(this, UniSpyEncoding.GetString(buffer));
            switcher.Handle();
        }
    }
}