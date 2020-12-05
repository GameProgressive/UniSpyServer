using System.Linq;
using NATNegotiation.Abstraction.BaseClass;
using ServerBrowser.Entity.Enumerate;
using ServerBrowser.Entity.Structure.Request;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Logging;

namespace ServerBrowser.Handler.CommandSwitcher
{
    public class SBRequestSerializer : UniSpyRequestSerializerBase
    {
        protected new byte[] _rawRequest
        {
            get { return (byte[])base._rawRequest; }
            set { base._rawRequest = value; }
        }

        public SBRequestSerializer(byte[] rawRequest) : base(rawRequest)
        {
        }

        public override IUniSpyRequest Serialize()
        {
            if (_rawRequest.Take(6).SequenceEqual(NNRequestBase.MagicData))
            {
                return new AdHocRequest(_rawRequest);
            }

            switch ((SBClientRequestType)_rawRequest[2])
            {
                case SBClientRequestType.ServerListRequest:
                    return new ServerListRequest(_rawRequest);
                case SBClientRequestType.ServerInfoRequest:
                    return new AdHocRequest(_rawRequest);
                case SBClientRequestType.SendMessageRequest:
                    //TODO Cryptorx's game use this command
                    return new AdHocRequest(_rawRequest);
                case SBClientRequestType.PlayerSearchRequest:
                    return null;
                case SBClientRequestType.MapLoopRequest:
                    return null;
                default:
                    LogWriter.UnknownDataRecieved(_rawRequest);
                    return null;
            }
        }
    }
}
