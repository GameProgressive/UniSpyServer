using System;
using System.Linq;
using UniSpyServer.Servers.QueryReport.Entity.Enumerate;
using UniSpyServer.Servers.QueryReport.Entity.Exception;

namespace UniSpyServer.Servers.QueryReport.Abstraction.BaseClass
{
    public abstract class RequestBase : UniSpyLib.Abstraction.BaseClass.RequestBase
    {
        public static readonly byte[] MagicData = { 0xFE, 0XFD };
        public uint? InstantKey { get; protected set; }
        public new RequestType CommandName { get => (RequestType)base.CommandName; set => base.CommandName = value; }
        public new byte[] RawRequest { get => (byte[])base.RawRequest; protected set => base.RawRequest = value; }

        public RequestBase(object rawRequest) : base(rawRequest)
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
