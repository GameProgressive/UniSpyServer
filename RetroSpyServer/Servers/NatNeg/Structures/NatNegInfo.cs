using System;
using System.Collections.Generic;
using System.Text;

namespace RetroSpyServer.Servers.NatNeg.Structures
{
    public static class NatNegInfo
    {
 
        public static int FINISHED_NOERROR = 0;

        public static int FINISHED_ERROR_DEADBEAT_PARTNER = 1;

        public static int FINISHED_ERROR_INIT_PACKETS_TIMEDOUT = 2;

        public static int PREINIT_RETRY_TIME = 500;

        public static int PREINIT_RETRY_COUNT = 10;

        public static int PREINITACK_RETRY_TIME = 10000;

        public static int PREINITACK_RETRY_COUNT = 12;

        public static int INIT_RETRY_TIME = 500;

        public static int INIT_RETRY_COUNT = 10;

        public static int NNINBUF_LEN = 512;

        public static int PING_RETRY_TIME = 700;

        public static int PING_RETRY_COUNT = 7;

        public static int PARTNER_WAIT_TIME = 60000;
        public static int REPORT_RETRY_TIME = 500;
        public static int REPORT_RETRY_COUNT = 4;


        public const int NN_PROTVER = 4;


        //public static int NN_PROTVER 3
        //public static int NN_PROTVER 2

        public const int NN_PT_GP = 0;

        public const int NN_PT_NN1 = 1;

        public const int NN_PT_NN2 = 2;

        public const int NN_PT_NN3 = 3;


        

        public static int NN_PREINIT_WAITING_FOR_CLIENT = 0;

        public static int NN_PREINIT_WAITING_FOR_MATCHUP = 1;

        public static int NN_PREINIT_READY = 2;

        public const int BASEPACKET_SIZE = 12;

        public const int BASEPACKET_TYPE_OFFSET = 7;

        public const int INITPACKET_SIZE = BASEPACKET_SIZE + 9;

        public const int INITPACKET_ADDRESS_OFFSET = BASEPACKET_SIZE + 3;

        public const int REPORTPACKET_SIZE = BASEPACKET_SIZE + 61;

        public const int CONNECTPACKET_SIZE = BASEPACKET_SIZE + 8;

        public const int PREINITPACKET_SIZE = BASEPACKET_SIZE + 6;

        public static int NNCookieType;
    }
}
