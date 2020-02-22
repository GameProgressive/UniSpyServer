using System;
namespace QueryReport.Entity.Structure.Packet
{
    public class QueryPacket
    {
        public static readonly byte[] QRMagicData = { 0xFE, 0xFD };
        public QueryPacket(byte[] recv)
        {
        }
        
    }
}
