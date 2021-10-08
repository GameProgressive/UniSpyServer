using System;
using System.Xml.Serialization;

namespace WebServer.Entity.Structure
{
    [XmlType(IncludeInSchema = false)]
    public record RecordDataValue<T>
    {
        [XmlElement("value")]
        public T Value;
    }

    [XmlType(IncludeInSchema = false)]
    public record RecordValue
    {
        [XmlElement("binaryDataValue")]
        public RecordDataValue<string> BinaryValue; // BASE64

        [XmlElement("intValue")]
        public RecordDataValue<int> IntValue;

        [XmlElement("byteValue")]
        public RecordDataValue<byte> ByteValue;

        [XmlElement("shortValue")]
        public RecordDataValue<short> ShortValue;

        [XmlElement("int64Value")]
        public RecordDataValue<long> Int64Value;

        [XmlElement("floatValue")]
        public RecordDataValue<float> FloatValue;

        [XmlElement("asciiStringValue")]
        public RecordDataValue<string> AsciiStringValue; // Encoding.ASCII

        [XmlElement("unicodeStringValue")]
        public RecordDataValue<string> UnicodeStringValue; // Encoding.UTF16

        [XmlElement("booleanValue")]
        public RecordDataValue<bool> BoolValue;

        [XmlElement("dateAndTimeValue")]
        public RecordDataValue<DateTime> DateAndTimeValue;
    }
}
