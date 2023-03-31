using System;
using System.Net;
using Newtonsoft.Json;
using UniSpy.Server.QueryReport.Abstraction.BaseClass;
using UniSpy.Server.Core.Misc;
using System.Linq;

namespace UniSpy.Server.QueryReport.Contract.Request
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
