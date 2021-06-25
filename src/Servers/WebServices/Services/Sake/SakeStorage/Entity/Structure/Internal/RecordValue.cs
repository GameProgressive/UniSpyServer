using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace UniSpyWebService.Models.SakeStorageServer.Response
{
    public record ByteValue
    {
        public byte value;
    }

    public record ShortValue
    {
        public ushort value;
    }

    public record IntValue
    {
        public int value;
    }

    public record Int64Value
    {
        public long value;
    }

    public record FloatValue
    {
        public float value;
    }

    public record AsciiStringValue
    {
        public string value; // Encoding.Ascii
    }

    public record UnicodeStringValue
    {
        public string value; // Encoding.UTF8
    }

    public record BooleanValue
    {
        public bool value; // true/false actually
    }

    public record DateAndTimeValue
    {
        public DateTime value;
    }

    public record BinaryDataValue
    {
        public string value; // Encoding.Ascii + Base64 !
    }

    public record RecordValue
    {
        [XmlElement]
        public ByteValue byteValue;

        [XmlElement]
        public ShortValue shortValue;

        [XmlElement]
        public IntValue intValue;

        [XmlElement]
        public Int64Value int64Value;

        [XmlElement]
        public FloatValue floatValue;

        [XmlElement]
        public AsciiStringValue asciiStringValue;

        [XmlElement]
        public UnicodeStringValue unicodeStringValue;

        [XmlElement]
        public BooleanValue booleanValue;

        [XmlElement]
        public DateAndTimeValue dateAndTimeValue;

        [XmlElement]
        public BinaryDataValue binaryDataValue;
    }

}
