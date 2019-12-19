using System;
using PresenceConnectionManager.Enumerator;

namespace PresenceConnectionManager.Handler.Error
{
    public class ErrorSender
    {
        /// <summary>
        /// Send a presence error
        /// </summary>
        /// <param name="session">The stream that will receive the error</param>
        /// <param name="errorCode">The error code</param>
        /// <param name="operationID">The operation id</param>
        public static void SendGPCMError(GPCMSession session, GPErrorCode errorCode, uint operationID)
        {
            string errorMsg = ErrorMsg.GetErrorMsg(errorCode);
            string sendingBuffer = string.Format(@"\error\\err\{0}\fatal\\errmsg\{1}\id\{2}\final\", (uint)errorCode, errorMsg, operationID);
            session.SendAsync(sendingBuffer);
        }
    }
}
