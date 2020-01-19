using System;
using System.Collections.Generic;
using System.Net;
using GameSpyLib.Common;
using QueryReport.Entity.Enumerator;
using QueryReport.Server;

namespace QueryReport.Handler.CommandHandler
{
    public class QRHandlerBase : HandlerBase<QRServer, byte[]>
    {
        protected byte[] _recv;
        protected List<Dictionary<string, object>> _result;
        protected byte[] _sendingBuffer;
        protected QRErrorCode _error = QRErrorCode.NoError;

        protected QRHandlerBase(byte[] recv)
        {
            _recv = recv;
        }

        public override void Handle(QRServer server)
        {
            CheckRequest(server);
            if (_error != QRErrorCode.NoError)
            {

                return;
            }
            DatabaseOperation(server);
            if (_error != QRErrorCode.NoError)
            {
                return;
            }
            ConstructResponse(server);
            if(_error!=QRErrorCode.NoError)
            {
                return;
            }
            SendResponse(server);
        }


        protected virtual void CheckRequest(QRServer server)
        { }
        protected virtual void DatabaseOperation(QRServer server)
        { }
        protected virtual void ConstructResponse(QRServer server)
        { }
        protected virtual void SendResponse(QRServer server)
        {
            if (_sendingBuffer != null)
            {
                server.SendAsync(server.Socket.RemoteEndPoint, _sendingBuffer);
            }
        }
    }
}
