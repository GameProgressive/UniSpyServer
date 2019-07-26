using System;
using System.Collections.Generic;
using System.Text;

namespace RetroSpyServer.Servers.NatNeg.Structures
{
    public static class NatNegInfo
    {
        public static bool HavePreInitData(byte version)
        {
            return version > 3;
        }

        public static readonly byte[] MagicData = { 0xFD, 0xFC, 0x1E, 0x66, 0x6A, 0x82 };

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

        public const int NN_PT_GP = 0;

        public const int NN_PT_NN1 = 1;

        public const int NN_PT_NN2 = 2;

        public const int NN_PT_NN3 = 3;

        public const int NN_PREINIT_WAITING_FOR_CLIENT = 0;

        public const int NN_PREINIT_WAITING_FOR_MATCHUP = 1;

        public const int NN_PREINIT_READY = 2;

        public static int NNCookieType;
    }
}
