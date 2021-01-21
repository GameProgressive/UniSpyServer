using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure.Request;
using UniSpyLib.Abstraction.Interface;

namespace Chat.Handler.CmdHandler.General
{
    /// <summary>
    /// currently we do not know how this work
    /// so we do not implement it
    /// </summary>
    public class GETUDPRELAYHandler : ChatCmdHandlerBase
    {
        new GETUDPRELAYRequest _request { get { return (GETUDPRELAYRequest)base._request; } }

        public GETUDPRELAYHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

    }
}
