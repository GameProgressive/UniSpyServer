using System.Net;
using UniSpy.Server.ServerBrowser.V2.Entity.Enumerate;
namespace UniSpy.Server.ServerBrowser.V2.Abstraction.BaseClass
{
    public abstract class ServerListUpdateOptionRequestBase : RequestBase
    {
        public byte? RequestVersion { get; protected set; }
        public byte? ProtocolVersion { get; protected set; }
        public byte? EncodingVersion { get; protected set; }
        public int? GameVersion { get; protected set; }
        public int? QueryOptions { get; protected set; }
        public string DevGameName { get; protected set; }
        public string GameName { get; protected set; }
        public string ClientChallenge { get; protected set; }
        public ServerListUpdateOption? UpdateOption { get; protected set; }
        public string[] Keys { get; protected set; }
        public string Filter { get; protected set; }
        public IPAddress SourceIP { get; protected set; }
        public int? MaxServers { get; protected set; }
        protected ServerListUpdateOptionRequestBase(byte[] rawRequest) : base(rawRequest)
        {
        }


    }
}
