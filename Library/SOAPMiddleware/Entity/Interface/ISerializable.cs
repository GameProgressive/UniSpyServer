using System;
namespace SOAPMiddleware.Entity.Interface
{
    public interface ISoapSerializable
    {
        bool IsStartElement();
        void ReadStartElement();
        void Serilize();
        void Deserilize();
    }
}
