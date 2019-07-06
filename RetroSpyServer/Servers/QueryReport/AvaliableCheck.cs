using System.Text;
using GameSpyLib.Network;

namespace RetroSpyServer.Servers.QueryReport
{
    public enum ServerAvaliability : byte
    {
        Avaliable = 0x00,
        PermanentUnavaliable = 0x01,
        TemporarilyUnavaliable = 0x02,
    };

    public class AvaliableCheck
    {
        private static readonly byte[] AvailableReplyPrefix = { 0xfe, 0xfd, 0x09 };
        private static readonly byte[] AvailableReplyPostfix = { 0x00, 0x00, 0x00 };

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
            return request[request.Length] == AvailableCheckRequestPostfix;
        }

        public static void CheckForGameAvaliability(QRServer server, UdpPacket packet)
        {
            if (!IsClientRequestValid(packet.BytesRecieved))
            {
                // ???
                return;
            }

            //string gameName = GetGameName(packet.BytesRecieved);

            byte[] dataToSend = new byte[7];
            AvailableReplyPrefix.CopyTo(dataToSend, 0);
            AvailableReplyPostfix.CopyTo(dataToSend, 4);

            // NOTE: Change this if you want to make the server not avaliable.
            dataToSend[3] = (byte)ServerAvaliability.Avaliable;

        }

        /// <summary>
        /// get the gamename in avaliableCheckRequest that sended by client
        /// </summary>
        /// <param name="avaliableCheckRequest"></param>
        /// <returns></returns>
        private static string GetGameName(byte[] avaliableCheckRequest)
        {
            string tempRequest = Encoding.Default.GetString(avaliableCheckRequest);
            ////delete the frist 6 bytes and last byte
            string gameNameInGameSpy = tempRequest.Substring(5, tempRequest.Length - 6);
            return gameNameInGameSpy;
        }
    }
}
