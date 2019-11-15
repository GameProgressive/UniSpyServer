using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Handler.AuthHandler.Struct
{
    [DataContract]
    public class AuthenticationRequest
    {
        [DataMember]
        public string StringProperty { get; set; }

        [DataMember]
        public int IntProperty { get; set; }

        [DataMember]
        public List<string> ListProperty { get; set; }

        [DataMember]
        public DateTimeOffset DateTimeOffsetProperty { get; set; }

        [DataMember]
        public List<ComplexObject> ComplexListProperty { get; set; }
    }
}