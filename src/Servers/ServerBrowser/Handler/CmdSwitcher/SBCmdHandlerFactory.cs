using ServerBrowser.Entity.Enumerate;
using ServerBrowser.Entity.Structure.Request;
using ServerBrowser.Handler.CmdHandler;
using System;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;

namespace ServerBrowser.Handler.CommandSwitcher
{
    public class SBCmdHandlerFactory : UniSpyCmdHandlerBase
    {
        public SBCmdHandlerFactory(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        public override IUniSpyHandler Deserialize()
        {
            //we do not need to handle GOA query because it is handled by game server
            switch (_request.CommandName)
            {
                case RequestType.ServerListRequest:
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
                case RequestType.ServerInfoRequest:
                    return new ServerInfoHandler(_session, _request);
                case RequestType.PlayerSearchRequest:
                    goto default;
                case RequestType.MapLoopRequest:
                    goto default;
                case RequestType.SendMessageRequest:
                    //TODO
                    //Cryptorx's game use this command
                    return new SendMessageHandler(_session, _request);
                case RequestType.NatNegRequest:
                    return new NatNegMsgHandler(_session, _request);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
