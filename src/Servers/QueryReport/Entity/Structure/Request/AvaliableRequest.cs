using QueryReport.Abstraction.BaseClass;
using QueryReport.Entity.Exception;

namespace QueryReport.Entity.Structure.Request
{
    internal sealed class AvaliableRequest : QRRequestBase
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
                    throw new QRException("Avaliable request prefix is invalid.");
                }
            }

            //postfix check
            if (RawRequest[RawRequest.Length - 1] != AvaliableRequest.Postfix)
            {
                throw new QRException("Avaliable request postfix is invalid.");
            }
        }
    }
}
