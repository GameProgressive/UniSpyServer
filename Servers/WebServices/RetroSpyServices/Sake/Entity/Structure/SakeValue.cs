using System;
using System.Runtime.Serialization;

namespace WebServices.RetroSpyServices.Sake.Entity.Structure.Model
{
    public class SakeValue
    {
        [DataMember(Name = SakeConstant.AsciiStringValue]
        public string AsciiStringValue;
    }
}
