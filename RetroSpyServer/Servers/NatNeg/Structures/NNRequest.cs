using System;
using System.Collections.Generic;
using System.Text;

namespace RetroSpyServer.Servers.NatNeg.Structures
{
    public static class NNRequest
    {
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

    }
}
