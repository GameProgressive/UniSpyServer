using System.Runtime.Serialization;

namespace Handler.SoapAuthHandler
{
    [DataContract]
    public class ComplexObject
    {
        [DataMember]
        public string StringProperty { get; set; }

        [DataMember]
        public int IntProperty { get; set; }
    }
}