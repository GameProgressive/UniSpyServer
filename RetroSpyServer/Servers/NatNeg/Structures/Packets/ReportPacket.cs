using System;
using System.Collections.Generic;
using System.Text;
using RetroSpyServer.Servers.NatNeg.Enumerators;
namespace RetroSpyServer.Servers.NatNeg.Structures
{
    public  class ReportPacket
    {
        public  byte portType;
        public   byte clientIndex;
        public  byte negResult;
        public    NatType natType;
        public   NatMappingScheme natMappingScheme;
        public   char[] gamename = new char[50];
    }
}
