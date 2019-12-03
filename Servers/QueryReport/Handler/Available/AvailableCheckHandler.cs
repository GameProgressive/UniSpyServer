using System.Net;
using System.Text;

namespace QueryReport.Handler.Available
{
    public enum ServerAvailability : byte
    {
        Available = 0x00,
        PermanentUnavailable = 0x01,
        TemporarilyUnavailable = 0x02,
    };
    /// <summary>
    /// AvailableCheckHandler
    /// </summary>
    public class AvailableCheckHandler
    {
        private static readonly byte[] AvailableReply = { 0xfe, 0xfd, 0x09, 0x00, 0x00, 0x00 };

        private static readonly byte[] AvailableCheckRequestPrefix = { 0x09, 0x00, 0x00, 0x00, 0x00 };
        private static readonly byte AvailableCheckRequestPostfix = 0x00;

        /// <summary>
        /// Check the Available Check Request send by the client correctness.
        /// </summary>
        /// <param name="str">AvailableCheckRequestString</param>
        protected static bool IsClientRequestValid(byte[] request)
        {
            //prefix check
            for (int i = 0; i < AvailableCheckRequestPrefix.Length; i++)
            {
                if (request[i] != AvailableCheckRequestPrefix[i])
                    return false;
            }

            //postfix check
            return request[request.Length - 1] == AvailableCheckRequestPostfix;
        }

        public static void BackendAvaliabilityResponse(QRServer server, EndPoint endPoint, byte[] buffer)
        {
            if (!IsClientRequestValid(buffer))
            {
                // ???
                return;
            }

            //string gameName = GetGameName(packet.BytesRecieved);

            byte[] sendingBuffer = new byte[7];
            AvailableReply.CopyTo(sendingBuffer, 0);

            // NOTE: Change this if you want to make the server not avaliable.
            sendingBuffer[6] = (byte)ServerAvailability.Available;

            //packet.SetBufferContents(dataToSend);
            server.SendAsync(endPoint, sendingBuffer);
        }

        /// <summary>
        /// get the gamename in avaliableCheckRequest that sended by client
        /// </summary>
        /// <param name="avaliableCheckRequest"></param>
        /// <returns></returns>
        private static string GetGameName(byte[] recv)
        {
            string tempRequest = Encoding.Default.GetString(recv);
            ////delete the frist 6 bytes and last byte
            string gameNameInGameSpy = tempRequest.Substring(5, tempRequest.Length - 6);
            return gameNameInGameSpy;
        }
    }
}
