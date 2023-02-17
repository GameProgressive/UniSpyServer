using System.Collections.Generic;
using UniSpy.Server.QueryReport.V2.Abstraction.BaseClass;
using UniSpy.Server.QueryReport.V2.Entity.Structure.Request;
using UniSpy.Server.QueryReport.V2.Entity.Structure.Result;
using UniSpy.Server.Core.Encryption;

namespace UniSpy.Server.QueryReport.V2.Entity.Structure.Response
{
    public sealed class ChallengeResponse : ResponseBase
    {
        private static string Message = "RetroSpy echo!";

        public ChallengeResponse(ChallengeRequest request, ChallengeResult result) : base(request, result)
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
