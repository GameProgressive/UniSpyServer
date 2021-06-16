using System;
using System.Collections.Generic;
using UniSpyLib.Abstraction.BaseClass;
// ReSharper disable All

namespace NATNegotiation.Abstraction.BaseClass
{
    internal abstract class NNResponseBase : UniSpyResponseBase
    {
        protected new NNRequestBase _request => (NNRequestBase)base._request;
        protected new NNResultBase _result => (NNResultBase)base._result;
        public new byte[] SendingBuffer
        {
            get => (byte[])base.SendingBuffer;
            protected set => base.SendingBuffer = value;
        }
        public NNResponseBase(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }
        public override void Build()
        {
            List<byte> data = new List<byte>();
            data.AddRange(NNRequestBase.MagicData);
            data.Add(_request.Version);
            data.Add((byte)_result.PacketType);
            data.AddRange(BitConverter.GetBytes(_request.Cookie));
            SendingBuffer = data.ToArray();
        }
    }
}
