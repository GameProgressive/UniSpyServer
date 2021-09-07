using NatNegotiation.Abstraction.BaseClass;
using ServerBrowser.Entity.Enumerate;
using ServerBrowser.Entity.Structure.Request;
using System.Linq;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Logging;

namespace ServerBrowser.Handler.CommandSwitcher
{
    internal sealed class SBRequestFactory : UniSpyRequestFactory
    {
        private new byte[] _rawRequest => (byte[])base._rawRequest;

        public SBRequestFactory(byte[] rawRequest) : base(rawRequest)
        {
        }

        public override IUniSpyRequest Deserialize()
        {
            if (_rawRequest.Take(6).SequenceEqual(RequestBase.MagicData))
            {
                return new AdHocRequest(_rawRequest);
            }

            switch ((RequestType)_rawRequest[2])
            {
                case RequestType.ServerListRequest:
                    return new ServerListRequest(_rawRequest);
                case RequestType.ServerInfoRequest:
                    return new AdHocRequest(_rawRequest);
                case RequestType.SendMessageRequest:
                    //TODO Cryptorx's game use this command
                    return new AdHocRequest(_rawRequest);
                case RequestType.PlayerSearchRequest:
                    return null;
                case RequestType.MapLoopRequest:
                    return null;
                default:
                    LogWriter.LogUnkownRequest(_rawRequest);
                    return null;
            }
        }
    }
}
