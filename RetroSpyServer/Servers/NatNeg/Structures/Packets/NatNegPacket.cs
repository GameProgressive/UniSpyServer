using RetroSpyServer.Servers.NatNeg.Enumerators;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RetroSpyServer.Servers.NatNeg.Structures
{
    public class NatNegPacket
    {
        public byte[] magic= new byte[NNMagicData.NATNEG_MAGIC_LEN];
        public byte version;
        public  byte packettype;
        public uint  cookie;     
        public Packet packet;

    }
    public class PreinitPacket
    {
        public byte clientIndex;
        public byte state;
        public uint clientID;
    }
    public class ReportPacket
    {
        public byte portType;
        public byte clientIndex;
        public byte negResult;
        public NatType natType;
        public NatMappingScheme natMappingScheme;
        public char[] gamename = new char[50];
    }
    public class ConnectPacket
    {
        public uint remoteIP;
        public ushort remotePort;
        public byte gotyourdata;
        public byte finished;
    }
    public class InitPacket
    {
        public byte portType;
        public byte clientIndex;
        public byte useGamePort;
        public uint localIp;
        public ushort localPort;
    }


    [StructLayout(LayoutKind.Explicit, Size = 8)]
   public  struct Packet
    {
        [FieldOffset(0)]
       public InitPacket Init;
        [FieldOffset(0)]
        public ConnectPacket Connect;
        [FieldOffset(0)]
        public ReportPacket Report;
        [FieldOffset(0)]
        public PreinitPacket Preinit;
    }
}
