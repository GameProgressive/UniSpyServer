using GameSpyLib.Common.Entity.Interface;
using QueryReport.Entity.Enumerator;

namespace QueryReport.Handler.CommandHandler.Available
{
    /// <summary>
    /// AvailableCheckHandler
    /// </summary>
    public class AvailableHandler : QRCommandHandlerBase
    {
        private static readonly byte[] AvailableReply = { 0xfe, 0xfd, 0x09, 0x00, 0x00, 0x00 };
        private static readonly byte[] AvailableCheckRequestPrefix = { 0x09, 0x00, 0x00, 0x00, 0x00 };
        private static readonly byte AvailableCheckRequestPostfix = 0x00;

        public AvailableHandler(IClient client, byte[] recv) : base(client, recv)
        {
        }

        protected override void CheckRequest()
        {
            //prefix check
            for (int i = 0; i < AvailableCheckRequestPrefix.Length; i++)
            {
                if (_recv[i] != AvailableCheckRequestPrefix[i])
                {
                    _errorCode = QRErrorCode.Parse;
                    return;
                }
            }

            //postfix check
            if (_recv[_recv.Length - 1] != AvailableCheckRequestPostfix)
            {
                _errorCode = QRErrorCode.Parse;
                return;
            }
        }

        protected override void ConstructeResponse()
        {
            _sendingBuffer = new byte[7];
            AvailableReply.CopyTo(_sendingBuffer, 0);

            // NOTE: Change this if you want to make the server not avaliable.
            _sendingBuffer[6] = (byte)ServerAvailability.Available;
        }
    }
}
