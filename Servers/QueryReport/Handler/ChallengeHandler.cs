using GameSpyLib.Logging;
using QueryReport.Structure;
using System;

namespace QueryReport.Handler
{
    public class ChallengeHandler
    {
        /// <summary>
        /// Our hardcoded Server Validation code
        /// </summary>
        private static readonly byte[] ServerValidateCode = {
                0x72, 0x62, 0x75, 0x67, 0x4a, 0x34, 0x34, 0x64, 0x34, 0x7a, 0x2b,
                0x66, 0x61, 0x78, 0x30, 0x2f, 0x74, 0x74, 0x56, 0x56, 0x46, 0x64,
                0x47, 0x62, 0x4d, 0x7a, 0x38, 0x41, 0x00
            };


        public static void ServerChallengeResponse(QRServer server, byte[] buffer)
        {
           
            byte[] challenge = new byte[90];
            byte[] instancekey = new byte[4];

            Array.Copy(buffer, 1, instancekey, 0, 4);         

            byte[] sendingbuffer = new byte[7];
            sendingbuffer[0] = QR.QRMagic1;
            sendingbuffer[1] = QR.QRMagic2;
            sendingbuffer[2] = QRGameServer.ClientRegistered;
            Array.Copy(instancekey, 0, sendingbuffer, 3, 4);

            server.SendAsync(server.Endpoint, sendingbuffer);
            
            LogWriter.Log.Write("[QR] No impliment function for ServerChallengeResponse!", LogLevel.Debug);
        }
    }
}
