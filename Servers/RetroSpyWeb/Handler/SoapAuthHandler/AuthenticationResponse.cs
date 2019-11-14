using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Handler.SoapAuthHandler
{
    [DataContract]
    public class AuthenticationResponse
    {
        [DataMember]
        public float FloatProperty { get; set; }

        [DataMember]
        public string StringProperty { get; set; }

        [DataMember]
        public List<string> ListProperty { get; set; }

        [DataMember]
        public DateTimeOffset DateTimeOffsetProperty { get; set; }

        [DataMember]
        public TestEnum TestEnum { get; set; }
    }

    public enum TestEnum
    {
        One,
        Two
    }
}
