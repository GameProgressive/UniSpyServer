using System.Xml.Serialization;
using UniSpyServer.WebServer.Abstraction;

namespace UniSpyServer.WebServer.Entity.Structure.Result
{
    [XmlRoot("GetMyRecordsResponse", Namespace = "http://gamespy.net/sake")]
    public class GetMyRecordResult : ResultBase
    {
        public GetMyRecordResult()
        {
        }
    }
}