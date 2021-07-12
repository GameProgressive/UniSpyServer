using System.Net;
using NetCoreServer;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Logging;

namespace UniSpyLib.Abstraction.BaseClass.Network.Http.Server
{
    public abstract class UniSpyHttpSession : HttpSession, IUniSpySession
    {
        protected UniSpyHttpSession(UniSpyHttpServer server) : base(server)
        {
        }

        public EndPoint RemoteEndPoint => Socket.RemoteEndPoint;
        public IPEndPoint RemoteIPEndPoint => (IPEndPoint)RemoteEndPoint;

        public bool BaseSendAsync(object buffer)
        {
            var bufferType = buffer.GetType();
            if (bufferType == typeof(HttpResponse))
            {
                base.SendResponseAsync((HttpResponse)buffer);
                return true;
            }
            else if (bufferType == typeof(string))
            {
                base.SendResponseBodyAsync((string)buffer);
                return true;
            }
            else
            {
                throw new UniSpyException("The buffer type is invalid");
            }
        }
        protected override void OnReceivedRequest(HttpRequest request)
        {
            base.OnReceivedRequest(request);
        }
        protected virtual void OnRecievedRequest(UniSpyRequest request)
        {

        }
        public bool SendAsync(object buffer)
        {
            var bufferType = buffer.GetType();
            if (bufferType == typeof(HttpResponse))
            {
                SendResponseAsync((HttpResponse)buffer);
                return true;
            }
            else if (bufferType == typeof(string))
            {
                SendResponseBodyAsync((string)buffer);
                return true;
            }
            else
            {
                throw new UniSpyException("The buffer type is invalid");
            }
        }
    }
}