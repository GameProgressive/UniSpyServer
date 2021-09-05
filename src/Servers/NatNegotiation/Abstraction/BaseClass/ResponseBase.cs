using System;
using System.Collections.Generic;
using UniSpyLib.Abstraction.BaseClass;
// ReSharper disable All

namespace NatNegotiation.Abstraction.BaseClass
{
    internal abstract class ResponseBase : UniSpyResponseBase
    {
        protected new RequestBase _request => (RequestBase)base._request;
        protected new ResultBase _result => (ResultBase)base._result;
        public new byte[] SendingBuffer
        {
            get => (byte[])base.SendingBuffer;
            protected set => base.SendingBuffer = value;
        }
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
