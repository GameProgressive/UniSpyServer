using System.IO;
using System.Xml.Serialization;
using UniSpyLib.Abstraction.BaseClass;

namespace WebServer.Abstraction
{
    public abstract class ResponseBase : UniSpyResponseBase
    {
        public ResponseBase(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

        public override void Build()
        {
            throw new System.NotImplementedException();
            var xmlNspace = new XmlSerializerNamespaces();
            xmlNspace.Add("ns1", "http://gamespy.net/sake");
            using (var writer = new StringWriter())
            {
                new XmlSerializer(_result.GetType()).Serialize(writer, _result, xmlNspace);
                SendingBuffer = writer.ToString();
            }
        }
    }
}