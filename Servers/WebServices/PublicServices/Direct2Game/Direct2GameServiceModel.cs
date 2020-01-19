using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
namespace PublicServices.Direct2Game
{
    [DataContract]
    public class Direct2GameServiceModel
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Email { get; set; }
    }
}