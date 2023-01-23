using System;
using System.Net;
using Newtonsoft.Json;
using UniSpyServer.Servers.QueryReport.Abstraction.BaseClass;
using UniSpyServer.UniSpyLib.MiscMethod;
using System.Linq;

namespace UniSpyServer.Servers.QueryReport.Entity.Structure.Request
{
    public sealed class ClientMessageRequest : RequestBase
    {
        public Guid ServerBrowserSenderId { get; init; }
        public new uint? InstantKey { get => base.InstantKey; set => base.InstantKey = value; }
        public byte[] NatNegMessage { get; init; }
        [JsonConverter(typeof(IPEndPointConverter))]
        public IPEndPoint TargetIPEndPoint { get; init; }
        /// <summary>
        /// query report server will check on MessageKey, so this must different than previous ones 
        /// </summary>
        /// <returns></returns>
        public int? MessageKey => new Random().Next(int.MinValue, int.MaxValue);
        public uint Cookie => BitConverter.ToUInt32(NatNegMessage.Skip(6).Take(4).ToArray());
        public ClientMessageRequest() : base(null)
        {
        }
    }
}
