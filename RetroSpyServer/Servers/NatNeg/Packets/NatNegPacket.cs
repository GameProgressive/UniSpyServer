using RetroSpyServer.Servers.NatNeg.Enumerators;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using GameSpyLib.Network;
using GameSpyLib.Extensions;

namespace RetroSpyServer.Servers.NatNeg.Structures
{

    public class NatNegPacket
    {
        private byte[] RecData;
        public byte[] magic = NNMagicData.MagicData;
        public byte version;
        public byte packettype;
        public int cookie;
        public Packet packet = new Packet();

        public NatNegPacket(byte[] data)
        {
            //7*10+3=73(byte)
            //FD - FC - 1E- 66 - 6A - B2 - 04 - 0C - 00 - 00 - 
            //03 - 09 - 02 - 00 - 00 - 00 - 00 - 00 - 00 - 00 - 
            //00 - 00 - 00 - 00 - 00 - 00 - 00 - 00 - 00 - 00 - 
            //00 - 00 - 00 - 00 - 00 - 00 - 00 - 00 - 00 - 00 - 
            //00 - 00 - 00 - 00 - 00 - 00 - 00 - 00 - 00 - 00 - 
            //00 - 00 - 00 - 00 - 00 - 00 - 00 - 00 - 00 - 00 - 
            //00 - 00 - 00 - 00 - 00 - 00 - 00 - 00 - 00 - 00 - 
            //00 - 00 - 00
            RecData = data;
            version = data[6];//04
            packettype = data[7];//0C
            byte[] cookie = new byte[sizeof(int)];
            Array.Copy(data, 8, cookie, 0, 4);//00 - 00 - 03 - 09
            this.cookie = BitConverter.ToInt32(cookie);

            //c# dont have enum and pointer so we must create 3 functions to assign
            //all value into three classes variables.
            CopyInitData();
            CopyConnectData();
            CopyReportData();
        }

        public void CopyInitData()
        {
            packet.Init.PortType = RecData[13];//02
            packet.Init.ClientIndex = RecData[14];//00
            packet.Init.UseGamePort = RecData[15];//00
            packet.Init.LocalIp
                = BitConverter.ToUInt32(ByteExtensions.SubBytes(RecData, 16, sizeof(uint)));//00 - 00 - 00 - 00
            packet.Init.LocalPort
                = BitConverter.ToUInt16(ByteExtensions.SubBytes(RecData, 20, sizeof(ushort)));//00 - 00
        }

        public void CopyConnectData()
        {
            packet.Connect.RemoteIP
                = BitConverter.ToUInt32(ByteExtensions.SubBytes(RecData, 13, sizeof(uint)));
            packet.Connect.RemotePort
                    = BitConverter.ToUInt16(ByteExtensions.SubBytes(RecData, 17, sizeof(ushort)));
            packet.Connect.GotYourData = RecData[19];
            packet.Connect.Finished = RecData[20];
        }

        public void CopyReportData()
        {
            packet.Report.PortType = RecData[13];
            packet.Report.ClientIndex = RecData[14];
            packet.Report.NegResult = RecData[15];

            byte[] tempNatType = ByteExtensions.SubBytes(RecData, 17, sizeof(int));
            packet.Report.NatType = (NatType)BitConverter.ToInt32(tempNatType,0);

            byte[] tempNatMappingScheme = ByteExtensions.SubBytes(RecData, 19, sizeof(int));
            packet.Report.NatMappingScheme = (NatMappingScheme)BitConverter.ToInt32(tempNatMappingScheme, 0);

            //get the gamename
            Array.Copy(RecData, 23, packet.Report.GameName, 0, 50);
        }
    }

    public class Packet
    {
        //public PreinitPacket Preinit;
        public InitPacket Init = new InitPacket();
        public ConnectPacket Connect = new ConnectPacket();
        public ReportPacket Report = new ReportPacket();
        public PreinitPacket PreInit = new PreinitPacket();
    }

    public class PreinitPacket
    {
        public byte clientIndex;
        public byte state;
        public uint clientID;
    }
    public class InitPacket
    {
        public byte PortType;
        public byte ClientIndex;
        public byte UseGamePort;
        public uint LocalIp;
        public ushort LocalPort;
    }

    public class ReportPacket
    {
        public byte PortType;
        public byte ClientIndex;
        public byte NegResult;
        public NatType NatType;
        public NatMappingScheme NatMappingScheme;
        public byte[] GameName = new byte[50];

    }
    public class ConnectPacket
    {
        public uint RemoteIP;
        public ushort RemotePort;
        public byte GotYourData;
        public byte Finished;
    }

}
