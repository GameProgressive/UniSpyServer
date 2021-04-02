using ServerBrowser.Entity.Enumerate;
using ServerBrowser.Entity.Structure.Request;
using ServerBrowser.Handler.CmdHandler;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Logging;

namespace ServerBrowser.Handler.CommandSwitcher
{
    public class SBCmdHandlerFactory : UniSpyCmdHandlerFactoryBase
    {
        public SBCmdHandlerFactory(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        public override IUniSpyHandler Serialize()
        {
            //we do not need to handle GOA query because it is handled by game server
            switch (_request.CommandName)
            {
                case SBClientRequestType.ServerListRequest:
                    switch (((ServerListRequest)_request).UpdateOption)
                    {
                        case SBServerListUpdateOption.ServerNetworkInfoList:
                            return new ServerNetworkInfoListHandler(_session, _request);
                        case SBServerListUpdateOption.ServerMainList:
                            return new ServerMainListHandler(_session, _request);
                        case SBServerListUpdateOption.P2PGroupRoomList:
                            return new P2PGroupRoomListHandler(_session, _request);
                        case SBServerListUpdateOption.LimitResultCount:
                            return new P2PServerMainListHandler(_session, _request);
                        case SBServerListUpdateOption.P2PServerMainList:
                            // worms 3d send this after join group room
                            // we should send adhoc servers which are in this room to worms3d
                            return new P2PServerMainListHandler(_session, _request);
                        default:
                            return null;
                    }
                case SBClientRequestType.ServerInfoRequest:
                    return new ServerInfoHandler(_session, _request);
                case SBClientRequestType.PlayerSearchRequest:
                    return null;
                case SBClientRequestType.MapLoopRequest:
                    return null;
                case SBClientRequestType.SendMessageRequest:
                    //TODO
                    //Cryptorx's game use this command
                    return new SendMessageHandler(_session, _request);
                case SBClientRequestType.NatNegRequest:
                    return new NatNegMsgHandler(_session, _request);
                default:
                    LogWriter.UnknownDataRecieved((byte[])_request.RawRequest);
                    return null;
            }
        }
    }
}
