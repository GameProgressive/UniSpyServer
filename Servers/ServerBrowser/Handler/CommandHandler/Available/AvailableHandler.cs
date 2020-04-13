using System;
using QueryReport.Entity.Enumerator;
using ServerBrowser.Entity.Enumerator;

namespace ServerBrowser.Handler.CommandHandler.Available
{
    public class AvailableHandler : CommandHandlerBase
    {
        private readonly byte[] AvailableReply = { 0xfe, 0xfd, 0x09, 0x00, 0x00, 0x00 };
        private readonly byte[] AvailableCheckRequestPrefix = { 0x09, 0x00, 0x00, 0x00, 0x00 };
        private readonly byte AvailableCheckRequestPostfix = 0x00;

        protected override void CheckRequest(SBSession session, byte[] recv)
        {
            base.CheckRequest(session, recv);
            //prefix check
            for (int i = 0; i < AvailableCheckRequestPrefix.Length; i++)
            {
                if (recv[i] != AvailableCheckRequestPrefix[i])
                {
                    _errorCode = SBErrorCode.Parse;
                    return;
                }
            }

            //postfix check
            if (recv[recv.Length - 1] != AvailableCheckRequestPostfix)
            {
                _errorCode = SBErrorCode.Parse;
                return;
            }
        }

        protected override void ConstructResponse(SBSession session, byte[] recv)
        {
            base.ConstructResponse(session, recv);
            _sendingBuffer = new byte[7];
            AvailableReply.CopyTo(_sendingBuffer, 0);

            // NOTE: Change this if you want to make the server not avaliable.
            _sendingBuffer[6] = (byte)ServerAvailability.Available;
        }

    }
}
