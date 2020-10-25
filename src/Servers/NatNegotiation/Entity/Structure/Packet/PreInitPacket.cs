namespace NatNegotiation.Entity.Structure.Packet
{
    public class PreInitPacket : BasePacket
    {
        public int CLientIndex;
        public int State;
        public int ClientID;

        public new static readonly int Size = BasePacket.Size + 6;

        public override bool Parse(byte[] recv)
        {
            return base.Parse(recv);
        }
    }
}
