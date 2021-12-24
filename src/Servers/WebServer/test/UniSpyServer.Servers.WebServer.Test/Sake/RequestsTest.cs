using Xunit;
using UniSpyServer.Servers.WebServer.Entity.Structure.Request.Sake;

namespace UniSpyServer.Servers.WebServer.Test.Sake
{
    public class RequestsTest
    {
        //
        // These are the SOAP requests of SAKE
        // Endpoint: {FQDN}/sake/
        //
        [Fact]
        public void GetRecordLimit()
        {
            var request = new GetRecordLimitRequest(RawRequests.GetRecordLimit);
            request.Parse();
            Assert.Equal("0", request.GameId.ToString());
            Assert.Equal("XXXXXX", request.SecretKey);
            Assert.Equal("xxxxxxxx_YYYYYYYYYY__", request.LoginTicket);
            Assert.Equal("nicks", request.TableId);
        }
        [Fact]
        public void RateRecord()
        {
            var request = new RateRecordRequest(RawRequests.RateRecord);
            request.Parse();
            Assert.Equal("0", request.GameId.ToString());
            Assert.Equal("XXXXXX", request.SecretKey);
            Assert.Equal("xxxxxxxx_YYYYYYYYYY__", request.LoginTicket);
            Assert.Equal("test", request.TableId);
            Assert.Equal("158", request.RecordId);
            Assert.Equal("200", request.Rating);
        }
        [Fact]
        public void GetRandomRecords()
        {
            var request = new GetRandomRecordsRequest(RawRequests.GetRandomRecords);
            request.Parse();
            Assert.Equal("0", request.GameId.ToString());
            Assert.Equal("XXXXXX", request.SecretKey);
            Assert.Equal("xxxxxxxx_YYYYYYYYYY__", request.LoginTicket);
            Assert.Equal("levels", request.TableId);
            Assert.Equal("1", request.Max);
            Assert.Equal("recordid", request.Fields[0].FieldName);
            Assert.Equal("string", request.Fields[0].FiledType);
            Assert.Equal("score", request.Fields[1].FieldName);
            Assert.Equal("string", request.Fields[1].FiledType);
        }
        [Fact]
        public void GetSpecificRecords()
        {
            var request = new GetSpecificRecordsRequest(RawRequests.GetSpecificRecords);
            request.Parse();
            Assert.Equal("0", request.GameId.ToString());
            Assert.Equal("XXXXXX", request.SecretKey);
            Assert.Equal("xxxxxxxx_YYYYYYYYYY__", request.LoginTicket);
            Assert.Equal("scores", request.TableId);
            Assert.Equal("1", request.RecordIds[0].FieldName);
            Assert.Equal("int", request.RecordIds[0].FiledType);
            Assert.Equal("2", request.RecordIds[1].FieldName);
            Assert.Equal("int", request.RecordIds[1].FiledType);
            Assert.Equal("4", request.RecordIds[2].FieldName);
            Assert.Equal("int", request.RecordIds[2].FiledType);
            Assert.Equal("5", request.RecordIds[3].FieldName);
            Assert.Equal("int", request.RecordIds[3].FiledType);
            Assert.Equal("recordid", request.Fields[0].FieldName);
            Assert.Equal("string", request.Fields[0].FiledType);
            Assert.Equal("ownerid", request.Fields[1].FieldName);
            Assert.Equal("string", request.Fields[1].FiledType);
            Assert.Equal("score", request.Fields[2].FieldName);
            Assert.Equal("string", request.Fields[2].FiledType);
        }
        [Fact]
        public void GetMyRecords()
        {
            var request = new GetMyRecordsRequest(RawRequests.GetMyRecords);
            request.Parse();
            Assert.Equal("0", request.GameId.ToString());
            Assert.Equal("XXXXXX", request.SecretKey);
            Assert.Equal("xxxxxxxx_YYYYYYYYYY__", request.LoginTicket);
            Assert.Equal("test", request.TableId);
            Assert.Equal("recordid", request.Fields[0].FieldName);
            Assert.Equal("string", request.Fields[0].FiledType);
            Assert.Equal("ownerid", request.Fields[1].FieldName);
            Assert.Equal("string", request.Fields[1].FiledType);
            Assert.Equal("MyByte", request.Fields[2].FieldName);
            Assert.Equal("string", request.Fields[2].FiledType);
            Assert.Equal("MyShort", request.Fields[3].FieldName);
            Assert.Equal("string", request.Fields[3].FiledType);
            Assert.Equal("MyInt", request.Fields[4].FieldName);
            Assert.Equal("string", request.Fields[4].FiledType);
            Assert.Equal("MyFloat", request.Fields[5].FieldName);
            Assert.Equal("string", request.Fields[5].FiledType);
            Assert.Equal("MyAsciiString", request.Fields[6].FieldName);
            Assert.Equal("string", request.Fields[6].FiledType);
            Assert.Equal("MyUnicodeString", request.Fields[7].FieldName);
            Assert.Equal("string", request.Fields[7].FiledType);
            Assert.Equal("MyBoolean", request.Fields[8].FieldName);
            Assert.Equal("string", request.Fields[8].FiledType);
            Assert.Equal("MyDateAndTime", request.Fields[9].FieldName);
            Assert.Equal("string", request.Fields[9].FiledType);
            Assert.Equal("MyBinaryData", request.Fields[10].FieldName);
            Assert.Equal("string", request.Fields[10].FiledType);
            Assert.Equal("MyFileID", request.Fields[11].FieldName);
            Assert.Equal("string", request.Fields[11].FiledType);
            Assert.Equal("num_ratings", request.Fields[12].FieldName);
            Assert.Equal("string", request.Fields[12].FiledType);
            Assert.Equal("average_rating", request.Fields[13].FieldName);
            Assert.Equal("string", request.Fields[13].FiledType);
        }
        [Fact]
        public void SearchForRecords()
        {
            var request = new SearchForRecordsRequest(RawRequests.SearchForRecords);
            request.Parse();
            Assert.Equal("0", request.GameId.ToString());
            Assert.Equal("XXXXXX", request.SecretKey);
            Assert.Equal("xxxxxxxx_YYYYYYYYYY__", request.LoginTicket);
            Assert.Equal("scores", request.TableId);
            Assert.Equal("", request.Filter);
            Assert.Equal("", request.Sort);
            Assert.Equal("0", request.Offset);
            Assert.Equal("3", request.Max);
            Assert.Equal("0", request.Surrounding);
            Assert.Equal("", request.OwnerIds);
            Assert.Equal("0", request.CacheFlag);
            Assert.Equal("score", request.Fields[0].FieldName);
            Assert.Equal("string", request.Fields[0].FiledType);
            Assert.Equal("recordid", request.Fields[1].FieldName);
            Assert.Equal("string", request.Fields[1].FiledType);
        }
        [Fact]
        public void DeleteRecord()
        {
            var request = new DeleteRecordRequest(RawRequests.DeleteRecord);
            request.Parse();
            Assert.Equal("0", request.GameId.ToString());
            Assert.Equal("XXXXXX", request.SecretKey);
            Assert.Equal("xxxxxxxx_YYYYYYYYYY__", request.LoginTicket);
            Assert.Equal("test", request.TableId);
            Assert.Equal("150", request.RecordId);
        }
        //
        // TODO: Deserialization of RecordFields
        //
        [Fact]
        public void UpdateRecord()
        {
            var request = new UpdateRecordRequest(RawRequests.UpdateRecord);
            request.Parse();
            Assert.Equal("0", request.GameId.ToString());
            Assert.Equal("XXXXXX", request.SecretKey);
            Assert.Equal("xxxxxxxx_YYYYYYYYYY__", request.LoginTicket);
            Assert.Equal("test", request.TableId);
            Assert.Equal("158", request.RecordId);

            // Assert.Equal("score", request.Values[0].FieldName);
            // Assert.Equal("string", request.Values[0].FiledType);
            // Assert.Equal("recordid", request.Values[1].FieldName);
            // Assert.Equal("string", request.Values[1].FiledType);
        }
        //
        // TODO: Deserialization of RecordFields
        //
        [Fact]
        public void CreateRecord()
        {
            var request = new CreateRecordRequest(RawRequests.CreateRecord);
            request.Parse();
            Assert.Equal("0", request.GameId.ToString());
            Assert.Equal("XXXXXX", request.SecretKey);
            Assert.Equal("xxxxxxxx_YYYYYYYYYY__", request.LoginTicket);
            Assert.Equal("test", request.TableId);

            // Assert.Equal("score", request.Values[0].FieldName);
            // Assert.Equal("string", request.Values[0].FiledType);
            // Assert.Equal("recordid", request.Values[1].FieldName);
            // Assert.Equal("string", request.Values[1].FiledType);
        }
    }
}
