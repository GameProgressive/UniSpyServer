using System;
using System.Collections;
using System.Runtime.Serialization;

namespace WebServices.RetroSpyServices.Sake.Entity.Structure.Model.Basic
{
    public class SakeBasicValueType
    {
        [DataMember(Name = SakeXmlLable.AsciiStringValue)]
        public string AsciiStringValue;

        [DataMember(Name = SakeXmlLable.IntValue)]
        public int IntValue;

        [DataMember(Name = SakeXmlLable.Int64Value)]
        public long Int64Value;

        [DataMember(Name = SakeXmlLable.FloatValue)]
        public float FloatValue;

        [DataMember(Name = SakeXmlLable.UnicodeStringValue)]
        public float UnicodeStringValue;

        [DataMember(Name = SakeXmlLable.BooleanValue)]
        public bool BooleanValue;

        [DataMember(Name = SakeXmlLable.DateAndTimeValue)]
        public DateTime DateAndTimeValue;

        [DataMember(Name = SakeXmlLable.BinaryDataValue)]
        public BitArray BinaryDataValue;

        [DataMember(Name = SakeXmlLable.ByteValue)]
        public byte ByteValue;


    }
}
