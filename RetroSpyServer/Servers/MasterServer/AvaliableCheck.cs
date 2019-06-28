using System;
using System.Collections.Generic;
using System.Text;
using GameSpyLib.Network;
using GameSpyLib.Extensions;
using System.Collections;

namespace RetroSpyServer.Servers.MasterServer
{
    public class AvaliableCheck
    {
        /// <summary>
        /// BF2Available response
        /// </summary>
        private static readonly byte[] AvailableReply = { 0xfe, 0xfd, 0x09, 0x00, 0x00, 0x00, 0x00 };


        /// <summary>
        /// Our hardcoded Server Validation code
        /// </summary>
        private static readonly byte[] ServerValidateCode = {
                0x72, 0x62, 0x75, 0x67, 0x4a, 0x34, 0x34, 0x64, 0x34, 0x7a, 0x2b,
                0x66, 0x61, 0x78, 0x30, 0x2f, 0x74, 0x74, 0x56, 0x56, 0x46, 0x64,
                0x47, 0x62, 0x4d, 0x7a, 0x38, 0x41, 0x00
            };

        private static readonly byte[] AvailableCheckRequestPrefix = { 0x09, 0x00, 0x00, 0x00, 0x00 };
        private static readonly byte AvailableCheckRequestPostfix = 0x00 ;

        /// <summary>
        /// Check the Available Check Request send by the client correctness.
        /// </summary>
        /// <param name="str">AvailableCheckRequestString</param>
        public bool IsClientValid(byte[] request)
        {
            bool valid;
            //prefix check
            for (int i = 0; i < 6; i++)
            {
                if (request[i] == AvailableCheckRequestPrefix[i])
                { valid = true; }
                else
                { valid = false; }
            }
            //postfix check
            if (request[request.Length] == AvailableCheckRequestPostfix)
            { valid = true; }
            else
            { valid = false; }

            return valid;

        }

        /// <summary>
        /// Check the delicated server available check request correct.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public byte[] IsServerValid(byte[] request)
        {
            byte[] uniqueId = new byte[4];
            Array.Copy(request, 1, uniqueId, 0, 4);
            return null;

        }
        /// <summary>
        /// get the gamename in avaliableCheckRequest that sended by client
        /// </summary>
        /// <param name="avaliableCheckRequest"></param>
        /// <returns></returns>
        private byte[] GetGameName(byte[] avaliableCheckRequest)
        {
            //byte method to get gamename
            byte[] gameName = new byte[avaliableCheckRequest.Length - 6];
            for (int i = 5; i < avaliableCheckRequest.Length - 1; i++)
            {
                gameName[i - 5] = avaliableCheckRequest[i];
            }
            return gameName;

            /////
            ///// string method
            /////
            //string tempRequest = System.Text.Encoding.Default.GetString(avaliableCheckRequest);
            ////delete the frist 6 bytes and last byte
            //string gameNameInGameSpy = tempRequest.Substring(5, tempRequest.Length - 6);
            //return gameNameInGameSpy;
        }
    }
}
