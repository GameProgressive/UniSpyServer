using System;
using System.Net;
using Newtonsoft.Json;
using UniSpy.Server.Chat.Abstraction.Interface;
using UniSpy.Server.Chat.Application;
using UniSpy.Server.Chat.Exception;
using UniSpy.Server.Chat.Handler;
using UniSpy.Server.Core.Abstraction.BaseClass;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Core.Events;
using UniSpy.Server.Core.Logging;
using UniSpy.Server.Core.MiscMethod;

namespace UniSpy.Server.Chat.Aggregate
{
    public class ConcreteTypeConverter<TConcrete> : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            //assume we can convert to anything for now
            return true;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            //explicitly specify the concrete type we want to create
            return serializer.Deserialize<TConcrete>(reader);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            //use the default serialization - it works fine
            serializer.Serialize(writer, value);
        }
    }
    public class RemoteClient : IChatClient, Core.Abstraction.Interface.ITestClient
    {
        public bool IsLogRaw { get; set; }
        [JsonConverter(typeof(ConcreteTypeConverter<RemoteTcpConnection>))]
        public IConnection Connection { get; set; }
        public ICryptography Crypto => null;
        [JsonConverter(typeof(ConcreteTypeConverter<ClientInfo>))]
        public ClientInfoBase Info { get; set; }
        ClientInfo IChatClient.Info => (ClientInfo)Info;
        public RemoteClient() { }
        public RemoteClient(RemoteTcpConnection conn, ClientInfo info)
        {
            Connection = conn;
            // we need to copy the client info because it is a reference type
            Info = info.DeepCopy();
            (Info as ClientInfo).IsRemoteClient = true;
        }
        public void Send(IResponse response) => this.LogDebug("Ignore remote client Send() operation");
        public RemoteClient GetRemoteClient() => this;

        public void TestReceived(byte[] buffer)
        {
            this.LogNetworkReceiving(buffer);
            var switcher = new CmdSwitcher(this, buffer);
            switcher.Switch();
        }
    }
    public class RemoteTcpServer : IServer
    {
        public Guid ServerID { get; set; }
        public string ServerName { get; set; }
        [JsonProperty(NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [JsonConverter(typeof(IPEndPointConverter))]
        public IPEndPoint ListeningIPEndPoint { get; set; }
        [JsonProperty(NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [JsonConverter(typeof(IPEndPointConverter))]
        public IPEndPoint PublicIPEndPoint { get; set; }
        public void Start() => throw new ChatException("Remote tcp connection do not have this method.");
        public RemoteTcpServer() { }
        public RemoteTcpServer(IServer server)
        {
            ServerID = server.ServerID;
            ServerName = server.ServerName;
            ListeningIPEndPoint = server.ListeningIPEndPoint;
            PublicIPEndPoint = server.PublicIPEndPoint;
        }
    }

    public class RemoteTcpConnection : ITcpConnection
    {

        [JsonConverter(typeof(ConcreteTypeConverter<RemoteTcpServer>))]
        public IServer Server { get; set; }
        [JsonConverter(typeof(IPEndPointConverter))]
        public IPEndPoint RemoteIPEndPoint { get; set; }
        public NetworkConnectionType ConnectionType { get; set; }
#pragma warning disable CS0067
        public event OnConnectedEventHandler OnConnect;
#pragma warning disable CS0067
        public event OnDisconnectedEventHandler OnDisconnect;
#pragma warning disable CS0067
        public event OnReceivedEventHandler OnReceive;
        public RemoteTcpConnection() { }
        public RemoteTcpConnection(ITcpConnection conn, RemoteTcpServer server)
        {
            Server = server;
            RemoteIPEndPoint = conn.RemoteIPEndPoint;
            ConnectionType = conn.ConnectionType;
        }
        public void Disconnect() => LogWriter.LogDebug("Remote tcp connection do not have this method.");
        public void Send(string response) => LogWriter.LogDebug("Remote tcp connection do not have this method.");
        public void Send(byte[] response) => LogWriter.LogDebug("Remote tcp connection do not have this method.");
    }
}