using QueryReport.Entity.Enumerator;
using QueryReport.Handler.SystemHandler;
using QueryReport.Server;
using System.Net;

namespace QueryReport.Handler.CommandHandler
{
    public class QRHandlerBase
    {
        protected QRErrorCode _errorCode = QRErrorCode.NoError;
        protected byte[] _sendingBuffer;
        protected QRHandlerBase(QRServer server, EndPoint endPoint, byte[] recv)
        {
            Handle(server, endPoint, recv);
        }

        protected virtual void Handle(QRServer server, EndPoint endPoint, byte[] recv)
        {
            CheckRequest(server, endPoint, recv);
            if (_errorCode != QRErrorCode.NoError)
            {
                server.ToLog(ErrorMessage.ToMsg(_errorCode));
                return;
            }

            DataOperation(server, endPoint, recv);
            if (_errorCode == QRErrorCode.Database)
            {
                server.ToLog(ErrorMessage.ToMsg(_errorCode));
                return;
            }

            ConstructeResponse(server, endPoint, recv);
            if (_errorCode != QRErrorCode.NoError)
            {
                server.ToLog(ErrorMessage.ToMsg(_errorCode));
                return;
            }

            Response(server, endPoint, recv);
        }

        protected virtual void CheckRequest(QRServer server, EndPoint endPoint, byte[] recv) { }

        protected virtual void DataOperation(QRServer server, EndPoint endPoint, byte[] recv) { }

        protected virtual void ConstructeResponse(QRServer server, EndPoint endPoint, byte[] recv) { }

        protected virtual void Response(QRServer server, EndPoint endPoint, byte[] recv)
        {
            if (_sendingBuffer == null)
                return;
            server.SendAsync(endPoint, _sendingBuffer);
        }

    }
}
