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
        public byte[] Magic = NNMagicData.MagicData;
        public byte Version;
        public byte PacketType;
        //store cookie in int format
        public int Cookie;
        //stores cookie in bytes format
        public byte[] Byte_Cookie = new byte[sizeof(int)];
        public Packet Packet = new Packet();

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
            Version = data[6];//04
            PacketType = data[7];//0C
            byte[] cookie = new byte[sizeof(int)];
            Array.Copy(data, 8, cookie, 0, 4);//00 - 00 - 03 - 09
            Array.Copy(data, 8, Byte_Cookie, 0, 4);
            Cookie = BitConverter.ToInt32(cookie);

            //c# dont have enum and pointer so we must create 3 functions to assign
            //all value into three classes variables.
            CopyInitData();
            CopyConnectData();
            CopyReportData();
        }

        public void CopyInitData()
        {
            Packet.Init.PortType = RecData[13];//02
            Packet.Init.ClientIndex = RecData[14];//00
            Packet.Init.UseGamePort = RecData[15];//00
            Packet.Init.LocalIp
                = BitConverter.ToUInt32(ByteExtensions.SubBytes(RecData, 16, sizeof(uint)));//00 - 00 - 00 - 00
            Packet.Init.LocalPort
                = BitConverter.ToUInt16(ByteExtensions.SubBytes(RecData, 20, sizeof(ushort)));//00 - 00
        }

        public void CopyConnectData()
        {
            Packet.Connect.RemoteIP
                = BitConverter.ToUInt32(ByteExtensions.SubBytes(RecData, 13, sizeof(uint)));
            Packet.Connect.RemotePort
                    = BitConverter.ToUInt16(ByteExtensions.SubBytes(RecData, 17, sizeof(ushort)));
            Packet.Connect.GotYourData = RecData[19];
            Packet.Connect.Finished = RecData[20];
        }

        public void CopyReportData()
        {
            Packet.Report.PortType = RecData[13];
            Packet.Report.ClientIndex = RecData[14];
            Packet.Report.NegResult = RecData[15];

            byte[] tempNatType = ByteExtensions.SubBytes(RecData, 17, sizeof(int));
            Packet.Report.NatType = (NatType)BitConverter.ToInt32(tempNatType, 0);

            byte[] tempNatMappingScheme = ByteExtensions.SubBytes(RecData, 19, sizeof(int));
            Packet.Report.NatMappingScheme = (NatMappingScheme)BitConverter.ToInt32(tempNatMappingScheme, 0);

            //get the gamename
            Array.Copy(RecData, 23, Packet.Report.GameName, 0, 50);
        }

        /// <summary>
        /// Format the NatNeg packet to byte array ready for send back to client
        /// </summary>
        /// <param name="nnpacket"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public byte[] NatNegInitFormat(int size)
        {
            byte[] formatedNNPacket = new byte[size];
            Array.Copy(Magic,0, formatedNNPacket,0,Magic.Length);
            formatedNNPacket[6] = Version;
            formatedNNPacket[7] = PacketType;
            Array.Copy(Byte_Cookie, 0, formatedNNPacket, 8, 4);
            return formatedNNPacket;
        }

        public byte[] GetCookieBytes()
        {
            return BitConverter.GetBytes(Cookie);
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
        public byte ClientIndex;
        public byte State;
        public int ClientID;
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
