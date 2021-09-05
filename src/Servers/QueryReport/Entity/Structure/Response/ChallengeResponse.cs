using QueryReport.Abstraction.BaseClass;
using System.Collections.Generic;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Encryption;

namespace QueryReport.Entity.Structure.Response
{
    internal sealed class ChallengeResponse : QRResponseBase
    {
        private static string Message = "RetroSpy echo!";

        public ChallengeResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

        public override void Build()
        {
            base.Build();
            List<byte> data = new List<byte>();

            data.AddRange(SendingBuffer);
            data.AddRange(UniSpyEncoding.GetBytes(Message));

            SendingBuffer = data.ToArray();
        }
    }
}
