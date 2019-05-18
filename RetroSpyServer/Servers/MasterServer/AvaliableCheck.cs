using System;
using System.Collections.Generic;
using System.Text;
using GameSpyLib.Network;
namespace RetroSpyServer.Servers.MasterServer
{
    public class AvaliableCheck
    {
        /// <summary>
        /// GameSpy server available response
        /// </summary>
        private static readonly byte[] AvailableReply = { 0xfe, 0xfd, 0x09, 0x00, 0x00, 0x00, 0x00 };

        /// <summary>
        /// BF2Available Message. 09 then 4 00's then battlefield2
        /// </summary>
        private static readonly byte[] BF2AvailableRequest = {
                0x09, 0x00, 0x00, 0x00, 0x00, 0x62, 0x61, 0x74,
                0x74, 0x6c, 0x65, 0x66, 0x69, 0x65, 0x6c, 0x64, 0x32, 0x00
            };
        /// <summary>
        /// Crysis2Available Message
        /// </summary>
        private static readonly byte[] Crysis2AvailableRequest = {
                0x09, 0x00, 0x00, 0x00, 0x00, 0x63, 0x61, 0x70,
                0x72, 0x69, 0x63, 0x6F, 0x72, 0x6E, 0x00
            };

       
        public AvaliableCheck()
        {

        }
    }
}
