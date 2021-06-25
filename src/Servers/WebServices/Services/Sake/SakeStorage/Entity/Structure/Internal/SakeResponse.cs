using System.ServiceModel;
using System.Xml.Serialization;

namespace UniSpyWebService.Models.SakeStorageServer.Response
{
    /// <summary>
    /// <ArrayOfRecordValues>RecordValues....</ArrayOfRecordValues>
    /// </summary>
    public record ArrayOfValues
    {
        [XmlElement(IsNullable = false)]
        public RecordValue[] RecordValue;
    }

    /// <summary>
    /// <values>Array of values</values>
    /// </summary>
    public record Values
    {
        [XmlElement(IsNullable = false, ElementName = "ArrayOfRecordValue")]
        public ArrayOfValues values;
    }

    /// <summary>
    /// This will be the Success/Error text, has to work this way due to limitations of SOAPCore
    /// </summary>
    public record Result
    {
        [XmlText]
        public string Code;
    }

    /// <summary>
    /// Actual body of the XML response
    /// <XXXXXXResponse>....</XXXXXXResponse>
    /// </summary>
    public record Response
    {
        [XmlElement(ElementName = "SearchForRecordsResult", IsNullable = false)] // TODO: Change this with reflection?
        public Result result;

        [XmlElement(IsNullable = false)]
        public Values values;
    }

    /// <summary>
    /// Required or SOAPCore will serialize an extra class name
    /// </summary>
    [MessageContract(IsWrapped = false)]
    public record SakeResponse
    {
        [MessageBodyMember(Name = "SearchForRecordsResponse")] // TODO: Change this with reflection?
        public Response response;
    }
}
