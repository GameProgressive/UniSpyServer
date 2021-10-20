using System;
using System.Net;

namespace UniSpyServer.UniSpyLib.Extensions
{
    public class HtonsExtensions
    {
        public static byte[] UshortPortToHtonBytes(ushort port)
        {
            byte[] buffer = BitConverter.GetBytes(port);
            Array.Reverse(buffer);
            return buffer;
        }

        public static byte[] UshortPortToHtonBytes(string portStr)
        {
            ushort port = ushort.Parse(portStr);
            return UshortPortToHtonBytes(port);
        }

        public static byte[] UshortPortToBytes(string portStr)
        {
            ushort port = ushort.Parse(portStr);
            return UshortPortToBytes(port);
        }
        public static byte[] UshortPortToBytes(ushort port)
        {
            return BitConverter.GetBytes(port);
        }

        public static string EndPointToIP(EndPoint endPoint)
        {
            return ((IPEndPoint)endPoint).Address.ToString();
        }
        public static string EndPointToPort(EndPoint endPoint)
        {
            return ((IPEndPoint)endPoint).Port.ToString();
        }
        public static ushort EndPointToUshortPort(EndPoint endPoint)
        {
            return (ushort)((IPEndPoint)endPoint).Port;
        }
        public static byte[] EndPointToIPBytes(EndPoint endPoint)
        {
            return ((IPEndPoint)endPoint).Address.GetAddressBytes();
        }

        public static byte[] EndPointToHtonsPortBytes(EndPoint endPoint)
        {
            byte[] portBytes = BitConverter.GetBytes((short)((IPEndPoint)endPoint).Port);
            Array.Reverse(portBytes);
            return portBytes;
        }

        public static string BytesToIPString(byte[] ip)
        {
            return $"{ip[0]}.{ip[1]}.{ip[2]}.{ip[3]}";
        }

        public static byte[] IPStringToBytes(string ip)
        {
            return IPAddress.Parse(ip).GetAddressBytes();
        }
        public static byte[] IPStringToHtonBytes(string ip)
        {
            var buffer = IPStringToBytes(ip);
            Array.Reverse(buffer);
            return buffer;
        }
        public static byte[] IPToBytes(int ip)
        {
            return BitConverter.GetBytes(ip);
        }

        public static ushort HtonBytesToUshortPort(byte[] buffer)
        {
            Array.Reverse(buffer);
            return BytesToUshortPort(buffer);
        }

        public static ushort BytesToUshortPort(byte[] buffer)
        {
            return BitConverter.ToUInt16(buffer);
        }

        public static byte[] PortToIntBytes(string port)
        {
            return PortToIntBytes(int.Parse(port));
        }

        public static byte[] PortToIntBytes(int port)
        {
            return BitConverter.GetBytes(port);
        }
    }
}
