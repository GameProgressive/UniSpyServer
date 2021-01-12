using System.Collections.Generic;
using UniSpyLib.Extensions;
using NATNegotiation.Abstraction.BaseClass;
using NATNegotiation.Entity.Enumerate;
using UniSpyLib.Abstraction.BaseClass;
using NATNegotiation.Entity.Structure.Result;

namespace NATNegotiation.Entity.Structure.Response
{
    public class ConnectResponse : NNResponseBase
    {
        protected new ConnectResult _result
        {
            get { return (ConnectResult)base._result; }
        }
        public ConnectResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

        protected override void BuildNormalResponse()
        {
            List<byte> data = new List<byte>();

            data.AddRange(SendingBuffer);

            data.AddRange(HtonsExtensions.
                EndPointToIPBytes(_result.RemoteEndPoint));

            data.AddRange(HtonsExtensions.
                EndPointToHtonsPortBytes(_result.RemoteEndPoint));

            data.Add((byte)_result.GotYourData);
            data.Add((byte)_result.Finished);

            SendingBuffer = data.ToArray();
        }
    }
}
