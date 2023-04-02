using UniSpy.Server.QueryReport.V2.Abstraction.BaseClass;


namespace UniSpy.Server.QueryReport.V2.Contract.Request
{

    public sealed class AvaliableRequest : RequestBase
    {
        public static readonly byte[] Prefix = { 0x09, 0x00, 0x00, 0x00, 0x00 };
        public static readonly byte Postfix = 0x00;
        public AvaliableRequest(byte[] rawRequest) : base(rawRequest)
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
                    throw new QueryReport.Exception("Avaliable request prefix is invalid.");
                }
            }

            //postfix check
            if (RawRequest[RawRequest.Length - 1] != AvaliableRequest.Postfix)
            {
                throw new QueryReport.Exception("Avaliable request postfix is invalid.");
            }
        }
    }
}
