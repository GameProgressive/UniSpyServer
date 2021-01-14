using QueryReport.Entity.Enumerate;
using System;
using System.Collections.Generic;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Logging;

namespace QueryReport.Abstraction.BaseClass
{
    internal abstract class QRResponseBase : UniSpyResponseBase
    {
        protected new QRRequestBase _request => (QRRequestBase)base._request;
        protected new QRResultBase _result => (QRResultBase)base._result;
        public new byte[] SendingBuffer
        {
            get => (byte[])base.SendingBuffer;
            protected set => base.SendingBuffer = value;
        }
        protected QRResponseBase(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }
        public override void Build()
        {
            if (_result.ErrorCode != QRErrorCode.NoError)
            {
                BuildErrorResponse();
            }
            else
            {
                BuildNormalResponse();
            }
        }
        protected override void BuildErrorResponse()
        {
            LogWriter.ToLog(Serilog.Events.LogEventLevel.Error, _result.ErrorCode.ToString());
        }
        protected override void BuildNormalResponse()
        {
            List<byte> data = new List<byte>();
            data.AddRange(QRRequestBase.MagicData);
            data.Add((byte)_request.CommandName);
            data.AddRange(BitConverter.GetBytes(_request.InstantKey));

            SendingBuffer = data.ToArray();
        }
    }
}
