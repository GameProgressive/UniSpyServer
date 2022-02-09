using System;
using System.Linq;
using UniSpyServer.Servers.ServerBrowser.Entity.Enumerate;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;

namespace UniSpyServer.Servers.ServerBrowser.Abstraction.BaseClass
{
    public abstract class RequestBase : UniSpyLib.Abstraction.BaseClass.RequestBase
    {
        public int RequestLength { get; private set; }
        public new byte[] RawRequest => (byte[])base.RawRequest;
        public new RequestType CommandName { get => (RequestType)base.CommandName; protected set => base.CommandName = value; }
        public RequestBase(byte[] rawRequest) : base(rawRequest)
        {
        }

        public override void Parse()
        {
            RequestLength = BitConverter.ToInt16(RawRequest.Take(2).Reverse().ToArray());
            CommandName = (RequestType)RawRequest[2];
        }
    }
}
