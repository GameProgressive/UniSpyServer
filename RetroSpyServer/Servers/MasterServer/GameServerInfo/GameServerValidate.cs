using System;
using System.Collections.Generic;
using System.Text;

namespace RetroSpyServer.Servers.MasterServer.GameServerInfo
{
    public class GameServerValidate
    {
        /// <summary>
        /// Our hardcoded Server Validation code
        /// </summary>
        private readonly byte[] ServerValidateCode = {
                0x72, 0x62, 0x75, 0x67, 0x4a, 0x34, 0x34, 0x64, 0x34, 0x7a, 0x2b,
                0x66, 0x61, 0x78, 0x30, 0x2f, 0x74, 0x74, 0x56, 0x56, 0x46, 0x64,
                0x47, 0x62, 0x4d, 0x7a, 0x38, 0x41, 0x00
            };

        public GameServerValidate(byte[] clientChanllengeData)
        {
            ServerValidateChanllenge(clientChanllengeData);
        }


        public byte[] ServerValidateChanllenge(byte[] clientChanllengeData)
        {
            //To Do client challenge response

            return ServerValidateCode;
        }

    }
}
