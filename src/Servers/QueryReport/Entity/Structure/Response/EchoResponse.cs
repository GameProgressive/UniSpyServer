using System.Collections.Generic;
using System.Text;
using QueryReport.Abstraction.BaseClass;
using QueryReport.Entity.Structure.Request;
using QueryReport.Entity.Enumerate;

namespace QueryReport.Entity.Structure.Response
{
    public class EchoResponse : QRResponseBase
    {
        protected const string Message = "RetroSpy echo!";

        public EchoResponse(QRRequestBase request) : base(request)
        {
            PacketType = QRPacketType.Echo;
        }

        public EchoResponse(int instantKey) : base(QRPacketType.Echo, instantKey)
        {
        }

        public override byte[] BuildResponse()
        {
            List<byte> data = new List<byte>();

            data.AddRange(base.BuildResponse());
            data.AddRange(Encoding.ASCII.GetBytes(Message));

            return data.ToArray();
        }
    }
}
