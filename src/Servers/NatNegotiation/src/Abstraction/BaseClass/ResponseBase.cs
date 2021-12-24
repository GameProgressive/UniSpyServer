using System;
using System.Collections.Generic;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;
// ReSharper disable All

namespace UniSpyServer.Servers.NatNegotiation.Abstraction.BaseClass
{
    public abstract class ResponseBase : UniSpyResponseBase
    {
        protected new RequestBase _request => (RequestBase)base._request;
        protected new ResultBase _result => (ResultBase)base._result;
        public new byte[] SendingBuffer{ get => (byte[])base.SendingBuffer;
            protected set => base.SendingBuffer = value; }
        public ResponseBase(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }
        public override void Build()
        {
            List<byte> data = new List<byte>();
            data.AddRange(RequestBase.MagicData);
            data.Add(_request.Version);
            data.Add((byte)_result.PacketType);
            data.AddRange(BitConverter.GetBytes(_request.Cookie));
            SendingBuffer = data.ToArray();
        }
    }
}
