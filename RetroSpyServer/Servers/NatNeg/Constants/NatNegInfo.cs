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

        public static int FinishedNoError = 0;

        public static int FinishedErrorDeadBearPartner = 1;

        public static int FinishedErrorInitPacketsTimeOut = 2;
        
        public static int PreInitRetryTime = 500;

        public static int PreInitRetryCount = 10;

        public static int PreInitACKRetryTime = 10000;

        public static int PreInitACKRetryCount = 12;

        public static int InitRetryTime = 500;

        public static int InitRetryCount = 10;

        public static int NNInBufLen = 512;

        public static int PingRetryTime = 700;

        public static int PingRetryCount = 7;

        public static int PartnerWaitTime = 60000;

        public static int ReportRetryTime = 500;

        public static int ReportRetryCount = 4;

        public const int NNPTGP = 0;

        public const int NNPTNN1 = 1;

        public const int NNPTNN2 = 2;

        public const int NNPTNN3 = 3;

        public const int NNPreInitWaittingForClient = 0;

        public const int NNPreInitWaittingForMatchup = 1;

        public const int NNPreInitReady = 2;

        public static int NNCookieType;
    }
}
