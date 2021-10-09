using System;
using Xunit;
using WebServer.Entity.Structure.Request;
namespace UniSpyServer.Servers.WebServer.Test
{
    public class RequestDeserializationTest
    {
        [Fact]
        public void GetMyRecord()
        {
            var rawRequest = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><SOAP-ENV:Envelope xmlns:SOAP-ENV=\"http://schemas.xmlsoap.org/soap/envelope/\"    xmlns:SOAP-ENC=\"http://schemas.xmlsoap.org/soap/encoding/\"    xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"    xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:ns1=\"http://gamespy.net/sake\"><SOAP-ENV:Body><ns1:GetMyRecords><ns1:gameid>1687</ns1:gameid><ns1:secretKey>9r3Rmy</ns1:secretKey><ns1:loginTicket>xxxxxxxx_YYYYYYYYYY__</ns1:loginTicket><ns1:tableid>FriendInfo</ns1:tableid><ns1:fields><ns1:string>info</ns1:string><ns1:string>recordid</ns1:string></ns1:fields></ns1:GetMyRecords></SOAP-ENV:Body></SOAP-ENV:Envelope>";
            var request = new GetMyRecordRequest(rawRequest);
            request.Parse();
            Assert.Equal("1687", request.GameId.ToString());
            Assert.Equal("9r3Rmy", request.SecretKey);
            Assert.Equal("xxxxxxxx_YYYYYYYYYY__", request.LoginTicket);
            Assert.Equal("FriendInfo", request.TableId);
            Assert.Equal("info", request.Fields[0].FieldName);
            Assert.Equal("string", request.Fields[0].FiledType);
            
            Assert.Equal("recordid", request.Fields[1].FieldName);
            Assert.Equal("string", request.Fields[1].FiledType);
        }
    }
}
