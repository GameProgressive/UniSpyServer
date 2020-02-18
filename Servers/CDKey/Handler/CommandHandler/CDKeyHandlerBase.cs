using GameSpyLib.Common;
using System.Collections.Generic;

namespace CDKey.Handler.CommandHandler
{
    public class CDKeyHandlerBase : HandlerBase<CDKeyServer, Dictionary<string, string>>
    {
        protected Dictionary<string, string> _recv;
        protected string _sendingBuffer;
        public CDKeyHandlerBase(Dictionary<string, string> recv)
        {
            _recv = recv;
        }
        public override void Handle(CDKeyServer server)
        {
        }

        public virtual void CheckRequest(CDKeyServer server) { }

        public virtual void DataBaseOperation(CDKeyServer server) { }

        public virtual void CheckDatabaseResult(CDKeyServer server) { }

        public virtual void Response(CDKeyServer server)
        {
            if (_sendingBuffer != null)
            {
                server.SendAsync(server.Socket.RemoteEndPoint, _sendingBuffer);
            }
        }
    }
}
