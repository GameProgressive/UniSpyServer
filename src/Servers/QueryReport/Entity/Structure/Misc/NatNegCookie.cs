namespace QueryReport.Entity.Structure.NatNeg
{
    public class NatNegCookie
    {
        public string GameServerRemoteIP { get; set; }
        public string GameServerRemotePort { get; set; }
        public string GameName { get; set; }
        public byte[] NatNegMessage { get; set; }

        public NatNegCookie()
        {
        }
    }
}
