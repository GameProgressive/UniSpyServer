using System;
using System.Net;

namespace NATNegotiation.Handler.SystemHandler
{
    public class NNFormat
    {
        public static byte[] IPToByte(EndPoint endPoint)
        {
            return ((IPEndPoint)endPoint).Address.GetAddressBytes();
        }

        public static byte[] PortToByte(EndPoint endPoint)
        {
            return BitConverter.GetBytes(((IPEndPoint)endPoint).Port);
        }

        public static EndPoint IPToEndPoint(byte[] ip, byte[] port)
        {
            IPEndPoint end = new IPEndPoint(Convert.ToUInt32(ip), Convert.ToInt32(port));
            return end;
        }
    }
}
