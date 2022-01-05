using UniSpyServer.Servers.QueryReport.Abstraction.BaseClass;
using UniSpyServer.Servers.QueryReport.Entity.contract;
using UniSpyServer.Servers.QueryReport.Entity.Enumerate;
using UniSpyServer.Servers.QueryReport.Entity.Exception;

namespace UniSpyServer.Servers.QueryReport.Entity.Structure.Request
{
    [RequestContract(RequestType.AvaliableCheck)]
    public sealed class AvaliableRequest : RequestBase
    {
        public static readonly byte[] Prefix = { 0x09, 0x00, 0x00, 0x00, 0x00 };
        public static readonly byte Postfix = 0x00;
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
