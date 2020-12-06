using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure.ChatCommand;
using UniSpyLib.Abstraction.Interface;

namespace Chat.Handler.CommandHandler.ChatGeneralCommandHandler
{
    /// <summary>
    /// currently we do not know how this work
    /// so we do not implement it
    /// </summary>
    public class GETUDPRELAYHandler : ChatCommandHandlerBase
    {
        new GETUDPRELAYRequest _request { get { return (GETUDPRELAYRequest)base._request; } }

        public GETUDPRELAYHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

    }
}
