using NATNegotiation.Entity.Enumerate;
using Serilog.Events;
using System;
using System.Collections.Generic;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Logging;
// ReSharper disable All

namespace NATNegotiation.Abstraction.BaseClass
{
    public abstract class NNResponseBase : UniSpyResponseBase
    {
        protected new NNRequestBase _request => (NNRequestBase)base._request;

        protected new NNResultBase _result => (NNResultBase)base._result;

        public new byte[] SendingBuffer
        {
            get { return (byte[])base.SendingBuffer; }
            protected set { base.SendingBuffer = value; }
        }

        public NNResponseBase(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

        protected override void BuildNormalResponse()
        {
            List<byte> data = new List<byte>();
            data.AddRange(NNRequestBase.MagicData);
            data.Add(_request.Version);
            data.Add((byte)_result.PacketType);
            data.AddRange(BitConverter.GetBytes(_request.Cookie));
            SendingBuffer = data.ToArray();
        }

        protected override void BuildErrorResponse()
        {
            // now we only log error.
            LogWriter.ToLog(LogEventLevel.Error, _result.ErrorCode.ToString());
        }

        public override void Build()
        {
            if (_result.ErrorCode != NNErrorCode.NoError)
            {
                BuildErrorResponse();
            }
            else
            {
                BuildNormalResponse();
            }
        }
    }
}
