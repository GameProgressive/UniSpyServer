using System;
using System.Collections.Generic;
// ReSharper disable All

namespace UniSpy.Server.NatNegotiation.Abstraction.BaseClass
{
    public abstract class ResponseBase : UniSpy.Server.Core.Abstraction.BaseClass.ResponseBase
    {
        protected new RequestBase _request => (RequestBase)base._request;
        protected new ResultBase _result => (ResultBase)base._result;
        public new byte[] SendingBuffer
        {
            get => (byte[])base.SendingBuffer;
            protected set => base.SendingBuffer = value;
        }
        public ResponseBase(UniSpy.Server.Core.Abstraction.BaseClass.RequestBase request, UniSpy.Server.Core.Abstraction.BaseClass.ResultBase result) : base(request, result)
        {
        }
        public override void Build()
        {
            List<byte> data = new List<byte>();
            data.AddRange(RequestBase.MagicData);
            data.Add((byte)_request.Version);
            data.Add((byte)_result.PacketType);
            data.AddRange(BitConverter.GetBytes((uint)_request.Cookie));
            SendingBuffer = data.ToArray();
        }
    }
}
