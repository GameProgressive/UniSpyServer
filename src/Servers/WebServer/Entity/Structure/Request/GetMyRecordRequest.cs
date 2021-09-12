using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using WebServer.Abstraction;

namespace WebServer.Entity.Structure.Request
{
    [XmlRoot("GetMyRecords", Namespace = "http://gamespy.net/sake")]
    public class GetMyRecordRequest : RequestBase
    {
        [XmlElement("gameid")]
        public uint GameId { get; set; }

        [XmlElement("secretKey")]
        public string SecretKey { get; set; }

        [XmlElement("loginTicket")]
        public string LoginTicket { get; set; }

        [XmlElement("tableid")]
        public string TableId { get; set; }

        [XmlArray("fields")]
        [XmlArrayItem("string")]
        public string[] Fields { get; set; }
        public GetMyRecordRequest(string rawRequest) : base(rawRequest)
        {
        }

        public GetMyRecordRequest()
        {
        }

        public override void Parse()
        {
            var serializer = new XmlSerializer(this.GetType());
            var reader = XmlReader.Create(new StringReader(RawRequest));
            reader.ReadStartElement(); // SOAP-ENV:Envelope
            reader.ReadStartElement(); // SOAP-ENV:Body
            var request = (GetMyRecordRequest)serializer.Deserialize(reader);
            this.GameId = request.GameId;
            this.SecretKey = request.SecretKey;
            this.LoginTicket = request.LoginTicket;
            this.TableId = request.TableId;
            this.Fields = request.Fields;
        }
    }
}