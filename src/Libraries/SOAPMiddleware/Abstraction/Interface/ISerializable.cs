namespace SOAPMiddleware.Abstraction.Interface
{
    public interface ISoapSerializable
    {
        bool IsStartElement();
        void ReadStartElement();
        void Serilize();
        void Deserilize();
    }
}
