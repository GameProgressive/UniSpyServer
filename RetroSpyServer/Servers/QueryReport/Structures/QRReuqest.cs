using System;
using System.Collections.Generic;
using System.Text;

namespace RetroSpyServer.Servers.QueryReport.Structures
{
    public static class QRReuqest
    {
        public const byte KEEP_ALIVE = 0x08;
        public const byte HEART_BEAT = 0x03;
        public const byte ECHO = 0x05;
        public const byte AVAILABLE_CHECK = 0x09;
    }
}
