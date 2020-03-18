using QueryReport.Entity.Enumerator;
using QueryReport.Server;
using System.Net;

namespace QueryReport.Handler.CommandHandler.Available
{
    /// <summary>
    /// AvailableCheckHandler
    /// </summary>
    public class AvailableHandler : CommandHandlerBase
    {
        private readonly byte[] AvailableReply = { 0xfe, 0xfd, 0x09, 0x00, 0x00, 0x00 };
        private readonly byte[] AvailableCheckRequestPrefix = { 0x09, 0x00, 0x00, 0x00, 0x00 };
        private readonly byte AvailableCheckRequestPostfix = 0x00;

        public AvailableHandler(QRServer server, EndPoint endPoint, byte[] recv) : base(server, endPoint, recv)
        {
        }

        protected override void Handle(QRServer server, EndPoint endPoint, byte[] recv)
        {
            base.Handle(server, endPoint, recv);
        }

        protected override void CheckRequest(QRServer server, EndPoint endPoint, byte[] recv)
        {
            //prefix check
            for (int i = 0; i < AvailableCheckRequestPrefix.Length; i++)
            {
                if (recv[i] != AvailableCheckRequestPrefix[i])
                {
                    _errorCode = QRErrorCode.Parse;
                    return;
                }

            }

            //postfix check
            if (recv[recv.Length - 1] != AvailableCheckRequestPostfix)
            {
                _errorCode = QRErrorCode.Parse;
                return;
            }
        }

        protected override void ConstructeResponse(QRServer server, EndPoint endPoint, byte[] recv)
        {
            _sendingBuffer = new byte[7];
            AvailableReply.CopyTo(_sendingBuffer, 0);

            // NOTE: Change this if you want to make the server not avaliable.
            _sendingBuffer[6] = (byte)ServerAvailability.Available;
        }
    }
}
