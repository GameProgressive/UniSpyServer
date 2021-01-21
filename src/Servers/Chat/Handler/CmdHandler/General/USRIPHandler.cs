using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure.Request;
using Chat.Entity.Structure.Response.General;
using System.Net;
using UniSpyLib.Abstraction.Interface;

namespace Chat.Handler.CmdHandler.General
{
    public class USRIPHandler : ChatCmdHandlerBase
    {
        new USRIPRequest _request { get { return (USRIPRequest)base._request; } }
        public USRIPHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        protected override void BuildNormalResponse()
        {
            base.BuildNormalResponse();

            string ip = ((IPEndPoint)_session.Socket.RemoteEndPoint).Address.ToString();

            _sendingBuffer = USRIPReply.BuildUserIPReply(ip);
        }
    }
}
