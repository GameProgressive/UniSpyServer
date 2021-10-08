using System.Xml.Serialization;
using WebServer.Abstraction;

namespace WebServer.Entity.Structure.Result
{
    [XmlRoot("GetMyRecordsResponse", Namespace = "http://gamespy.net/sake")]
    public class GetMyRecordResult : ResultBase
    {
        public GetMyRecordResult()
        {
        }
    }
}