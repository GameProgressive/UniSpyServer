using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure.ChatCommand;
using GameSpyLib.Abstraction.Interface;

namespace Chat.Handler.CommandHandler.ChatGeneralCommandHandler
{
    /// <summary>
    /// currently we do not know how this work
    /// so we do not implement it
    /// </summary>
    public class GETUDPRELAYHandler : ChatCommandHandlerBase
    {
        new GETUDPRELAYRequest _request;

        public GETUDPRELAYHandler(ISession session, ChatRequestBase request) : base(session, request)
        {
            _request = (GETUDPRELAYRequest)request;
        }

    }
}
