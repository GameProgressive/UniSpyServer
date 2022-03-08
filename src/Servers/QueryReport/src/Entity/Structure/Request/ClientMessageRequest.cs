using System.Net;
using Newtonsoft.Json;
using UniSpyServer.Servers.QueryReport.Abstraction.BaseClass;
using UniSpyServer.Servers.QueryReport.Entity.contract;
using UniSpyServer.Servers.QueryReport.Entity.Enumerate;
using UniSpyServer.UniSpyLib.MiscMethod;

namespace UniSpyServer.Servers.QueryReport.Entity.Structure.Request
{
    [RequestContract(RequestType.ClientMessage)]
    public sealed class ClientMessageRequest : RequestBase
    {
        public new uint? InstantKey { get => base.InstantKey; set => base.InstantKey = value; }
        public byte[] Message { get; init; }
        [JsonConverter(typeof(IPEndPointConverter))]
        public IPEndPoint TargetIPEndPoint { get; init; }
        public readonly int? MessageKey = 0;
        public ClientMessageRequest() : base(null)
        {
        }
    }
}
