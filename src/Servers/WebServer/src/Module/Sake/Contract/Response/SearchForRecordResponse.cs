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
            _content.Add("SearchForRecordsResponse");
            _content.Add("SearchForRecordsResult", "Success");
            if (_result.UserData is not null)
            {
                var temp = Newtonsoft.Json.JsonConvert.DeserializeXNode(_result.UserData.RootElement.ToString()).Root;
                // add namespace to root element
                temp.Name = SakeSoapEnvelope.SakeNamespace + temp.Name.LocalName;
                // Add the namespace to each selected node
                foreach (var element in temp.DescendantsAndSelf())
                {
                    element.Name = SakeSoapEnvelope.SakeNamespace + element.Name.LocalName;
                }
                _content.Add("values", temp);
            }
            else
            {
                _content.Add("values");
            }

            base.Build();
        }
    }
}
