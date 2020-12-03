using System.Collections.Generic;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;

namespace ServerBrowser.Handler.CommandSwitcher
{
    public class SBRequestSerializer : RequestSerializerBase
    {
        protected new byte[] _rawRequest;
        public SBRequestSerializer(object rawRequest) : base(rawRequest)
        {
            _rawRequest = (byte[])rawRequest;
        }

        public override IRequest Serialize()
        {
            throw new System.NotImplementedException();
        }
    }
}
