using System.Net;
using NetCoreServer;
using UniSpyLib.Abstraction.Interface;

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

        public bool SendAsync(object buffer)
        {
            throw new System.NotImplementedException();
        }
    }
}