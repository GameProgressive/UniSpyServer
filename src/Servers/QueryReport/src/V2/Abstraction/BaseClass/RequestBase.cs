using System;
using System.Linq;
using UniSpy.Server.QueryReport.V2.Enumerate;
using UniSpy.Server.QueryReport.Exception;

namespace UniSpy.Server.QueryReport.V2.Abstraction.BaseClass
{
    public abstract class RequestBase : UniSpy.Server.Core.Abstraction.BaseClass.RequestBase
    {
        public static readonly byte[] MagicData = { 0xFE, 0XFD };
        public uint? InstantKey { get; protected set; }
        public new RequestType CommandName { get => (RequestType)base.CommandName; set => base.CommandName = value; }
        public new byte[] RawRequest { get => (byte[])base.RawRequest; protected set => base.RawRequest = value; }

        public RequestBase(byte[] rawRequest) : base(rawRequest)
        {
        }

        protected RequestBase()
        {
        }

        public override void Parse()
        {
            if (RawRequest.Length < 3)
            {
                throw new QRException("Query report request is invalid.");
            }
            CommandName = (RequestType)RawRequest[0];
            var instantKeyBytes = RawRequest.Skip(1).Take(4).ToArray();
            InstantKey = BitConverter.ToUInt32(instantKeyBytes);
        }
    }
}
