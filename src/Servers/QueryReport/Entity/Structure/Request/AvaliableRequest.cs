using QueryReport.Abstraction.BaseClass;

namespace QueryReport.Entity.Structure.Request
{
    public class AvaliableRequest : QRRequestBase
    {
        public static readonly byte[] Prefix = { 0x09, 0x00, 0x00, 0x00, 0x00 };
        public static readonly byte Postfix = 0x00;

        public AvaliableRequest(byte[] rawRequest) : base(rawRequest)
        {
        }

    }
}
