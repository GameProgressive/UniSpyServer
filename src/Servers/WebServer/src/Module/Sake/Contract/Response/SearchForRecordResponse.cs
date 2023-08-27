using System.Text;
using System.Xml.Linq;
using System.Text.Json;
using UniSpy.Server.WebServer.Module.Sake.Abstraction;
using UniSpy.Server.WebServer.Module.Sake.Contract.Request;
using UniSpy.Server.WebServer.Module.Sake.Contract.Result;

namespace UniSpy.Server.WebServer.Module.Sake.Contract.Response
{
    internal class SearchForRecordResponse : ResponseBase
    {
        private new SearchForRecordsRequest _request => (SearchForRecordsRequest)base._request;
        private new SearchForRecordsResult _result => (SearchForRecordsResult)base._result;
        public SearchForRecordResponse(SearchForRecordsRequest request, SearchForRecordsResult result) : base(request, result)
        {
        }

        public override void Build()
        {

            var newXele = Newtonsoft.Json.JsonConvert.DeserializeXNode(_result.UserData.RootElement.ToString()).Root;
            // add namespace to root element
            newXele.Name = SakeSoapEnvelope.SakeNamespace + newXele.Name.LocalName;
            // Add the namespace to each selected node
            foreach (var childNode in newXele.DescendantsAndSelf())
            {
                foreach (var element in childNode.Elements())
                {
                    element.Name = SakeSoapEnvelope.SakeNamespace + element.Name.LocalName;
                }
            }
            _content.Add("values", newXele);

            base.Build();
        }
    }
}
