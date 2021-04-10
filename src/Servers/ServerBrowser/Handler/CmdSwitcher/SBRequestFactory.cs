using NATNegotiation.Abstraction.BaseClass;
using ServerBrowser.Entity.Enumerate;
using ServerBrowser.Entity.Structure.Request;
using System.Linq;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Logging;

namespace ServerBrowser.Handler.CommandSwitcher
{
    internal sealed class SBRequestFactory : UniSpyRequestFactoryBase
    {
        private new byte[] _rawRequest => (byte[])base._rawRequest;

        public SBRequestFactory(byte[] rawRequest) : base(rawRequest)
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
                    LogWriter.LogUnkownRequest(_rawRequest);
                    return null;
            }
        }
    }
}
