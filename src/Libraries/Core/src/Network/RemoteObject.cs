using System;
using System.Net;
using Newtonsoft.Json;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Core.Events;
using UniSpy.Server.Core.Logging;
using UniSpy.Server.Core.Misc;

namespace UniSpy.Server.Core.Network
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
    public class RemoteServer : IServer
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        [JsonConverter(typeof(IPEndPointConverter))]
        public IPEndPoint ListeningIPEndPoint { get; private set; }
        [JsonConverter(typeof(IPEndPointConverter))]
        public IPEndPoint PublicIPEndPoint { get; private set; }
        public RemoteServer()
        {
        }
        public RemoteServer(IServer server)
        {
            Id = server.Id;
            Name = server.Name;
            ListeningIPEndPoint = server.ListeningIPEndPoint;
            PublicIPEndPoint = server.PublicIPEndPoint;
        }
        public void Start() { }
    }
    public class RemoteTcpConnectionManager : IConnectionManager
    {
#pragma warning disable CS0067
        public event OnConnectingEventHandler OnInitialization;
        public RemoteTcpConnectionManager() { }
        public void Start() { }
        public void Dispose() { }
    }

    public class RemoteTcpConnection : ITcpConnection
    {

        [JsonConverter(typeof(ConcreteTypeConverter<RemoteTcpConnectionManager>))]
        public IConnectionManager Manager { get; set; }
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
        public RemoteTcpConnection(ITcpConnection conn, IConnectionManager manager)
        {
            Manager = manager;
            RemoteIPEndPoint = conn.RemoteIPEndPoint;
            ConnectionType = conn.ConnectionType;
        }
        public void Disconnect() => LogWriter.LogDebug("Remote tcp connection do not have this method.");
        public void Send(string response) => LogWriter.LogDebug("Remote tcp connection do not have this method.");
        public void Send(byte[] response) => LogWriter.LogDebug("Remote tcp connection do not have this method.");

        public void Dispose() { }
    }
}