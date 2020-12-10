using ServerBrowser.Entity.Enumerate;
using ServerBrowser.Entity.Structure.Request;
using ServerBrowser.Handler.CmdHandler;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Logging;

namespace ServerBrowser.Handler.CommandSwitcher
{
    public class SBCmdHandlerSerializer : UniSpyCmdHandlerSerializerBase
    {
        public SBCmdHandlerSerializer(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        public override IUniSpyHandler Serialize()
        {
            //we do not need to handle GOA query because it is handled by game server
            switch (_request.CommandName)
            {
                case SBClientRequestType.ServerListRequest:
                   return UpdateOptionSerialize();
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
                    return new NatNegCookieHandler(_session, _request);
                default:
                    LogWriter.UnknownDataRecieved((byte[])_request.RawRequest);
                    return null;
            }
        }

        protected IUniSpyHandler UpdateOptionSerialize()
        {

            switch (((ServerListRequest)_request).UpdateOption)
            {
                case SBServerListUpdateOption.NoServerList:
                    return new NoServerListHandler(_session, _request);
                case SBServerListUpdateOption.GeneralRequest:
                    return new GeneralRequestHandler(_session, _request);
                case SBServerListUpdateOption.SendGroups:
                    return new SendGroupsHandler(_session, _request);
                case SBServerListUpdateOption.LimitResultCount:
                    return new PushUpdatesHandler(_session, _request);
                case SBServerListUpdateOption.PushUpdates:
                    // worms 3d send this after join group room
                    // we should send adhoc servers which are in this room to worms3d
                    return new PushUpdatesHandler(_session, _request);
                default:
                    return null;
            }
        }
    }
}
