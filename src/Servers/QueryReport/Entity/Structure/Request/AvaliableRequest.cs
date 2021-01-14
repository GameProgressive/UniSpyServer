using QueryReport.Abstraction.BaseClass;
using QueryReport.Entity.Enumerate;

namespace QueryReport.Entity.Structure.Request
{
    internal class AvaliableRequest : QRRequestBase
    {
        public static readonly byte[] Prefix = { 0x09, 0x00, 0x00, 0x00, 0x00 };
        public static readonly byte Postfix = 0x00;
        public static readonly string strPostfix = "0x00";
        public static readonly string strPrefix = "0x09, 0x00, 0x00, 0x00, 0x00";
        public AvaliableRequest(object rawRequest) : base(rawRequest)
        {
        }

        public override void Parse()
        {
            base.Parse();
            //prefix check
            for (int i = 0; i < AvaliableRequest.Prefix.Length; i++)
            {
                if (RawRequest[i] != AvaliableRequest.Prefix[i])
                {
                    ErrorCode = QRErrorCode.Parse;
                    return;
                }
            }

            //postfix check
            if (RawRequest[RawRequest.Length - 1] != AvaliableRequest.Postfix)
            {
                ErrorCode = QRErrorCode.Parse;
                return;
            }
        }
    }
}
