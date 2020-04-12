using System.Collections.Generic;
using GameSpyLib.Logging;
using Serilog.Events;

namespace CDKey.Handler.CommandHandler
{
    public class CommandHandlerBase
    {
        protected string _sendingBuffer;

        public CommandHandlerBase(CDKeyServer server, Dictionary<string, string> recv)
        {

        }

        public void Handle(CDKeyServer server, Dictionary<string, string> recv)
        {
            LogWriter.LogCurrentClass(this);

        }

        public virtual void CheckRequest(CDKeyServer server)
        {
        }

        public virtual void DataBaseOperation(CDKeyServer server)
        {
        }

        public virtual void CheckDatabaseResult(CDKeyServer server)
        {
        }

        public virtual void Response(CDKeyServer server)
        {
            if (_sendingBuffer == null || _sendingBuffer == "")
            {
                return;
            }
            server.SendAsync(server.Socket.RemoteEndPoint, _sendingBuffer);
        }
    }
}
