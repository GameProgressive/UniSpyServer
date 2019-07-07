using System;
using System.Collections.Generic;
using System.Text;

namespace RetroSpyServer.Servers.NatNeg.Structures
{
    public static class NatNegInfo
    {
        public static int NATNEG_MAGIC_LEN = 6;

        public static byte NN_MAGIC_0 = 0xFD;

        public static byte NN_MAGIC_1 = 0xFC;

        public static byte NN_MAGIC_2 = 0x1E;

        public static byte NN_MAGIC_3 = 0x66;

        public static byte NN_MAGIC_4 = 0x6A;

        public static byte NN_MAGIC_5 = 0xB2;

        public static byte[] NNMagicData = {
            NN_MAGIC_0,
            NN_MAGIC_1,
            NN_MAGIC_2,
            NN_MAGIC_3,
            NN_MAGIC_4,
            NN_MAGIC_5
        };

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


        public static int NN_PROTVER = 4;


        //public static int NN_PROTVER 3
        //public static int NN_PROTVER 2

        public static int NN_PT_GP = 0;

        public static int NN_PT_NN1 = 1;

        public static int NN_PT_NN2 = 2;

        public static int NN_PT_NN3 = 3;


        public const byte NN_INIT = 0x00;

        public const byte NN_INITACK = 0x01;

        public const byte NN_ERTTEST = 0x02;

        public const byte NN_ERTACK = 0x03;

        public const byte NN_STATEUPDATE = 0x04;

        public const byte NN_CONNECT = 0x05;

        public const byte NN_CONNECT_ACK = 0x06;

        public const byte NN_CONNECT_PING = 0x07;

        public const byte NN_BACKUP_TEST = 0x08;

        public const byte NN_BACKUP_ACK = 0x09;

        public const byte NN_ADDRESS_CHECK = 0x0A;

        public const byte NN_ADDRESS_REPLY = 0x0B;

        public const byte NN_NATIFY_REQUEST = 0x0C;

        public const byte NN_REPORT = 0x0D;

        public const byte NN_REPORT_ACK = 0x0E;

        public const byte NN_PREINIT = 0x0F;

        public const byte NN_PREINIT_ACK = 0x10;


        public static int NN_PREINIT_WAITING_FOR_CLIENT = 0;

        public static int NN_PREINIT_WAITING_FOR_MATCHUP = 1;

        public static int NN_PREINIT_READY = 2;

        public static int BASEPACKET_SIZE = 12;

        public static int BASEPACKET_TYPE_OFFSET = 7;

        public static int INITPACKET_SIZE = BASEPACKET_SIZE + 9;

        public static int INITPACKET_ADDRESS_OFFSET = BASEPACKET_SIZE + 3;

        public static int REPORTPACKET_SIZE = BASEPACKET_SIZE + 61;

        public static int CONNECTPACKET_SIZE = BASEPACKET_SIZE + 8;

        public static int PREINITPACKET_SIZE = BASEPACKET_SIZE + 6;

        public static int NNCookieType;
    }
}
