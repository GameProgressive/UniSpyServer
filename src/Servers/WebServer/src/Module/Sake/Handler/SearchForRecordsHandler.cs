using UniSpy.Server.WebServer.Abstraction;
using UniSpy.Server.WebServer.Module.Sake.Contract.Response;
using UniSpy.Server.WebServer.Module.Sake.Contract.Request;
using UniSpy.Server.WebServer.Application;
using UniSpy.Server.WebServer.Module.Sake.Contract.Result;
using UniSpy.Server.Core.Database.DatabaseModel;
using System.Linq;

namespace UniSpy.Server.WebServer.Module.Sake.Handler
{

    internal class SearchForRecordsHandler : CmdHandlerBase
    {
        private new SearchForRecordsRequest _request => (SearchForRecordsRequest)base._request;
        private new SearchForRecordsResult _result { get => (SearchForRecordsResult)base._result; set => base._result = value; }
        public SearchForRecordsHandler(Client client, SearchForRecordsRequest request) : base(client, request)
        {
            _result = new SearchForRecordsResult();
        }
        protected override void DataOperation()
        {
            base.DataOperation();
            // search user data from database
            using (var db = new UniSpyContext())
            {
                _result.UserData = db.Sakestorages.Where(
                t => t.Profileid == _request.OwnerIds
                && t.Tableid == _request.TableId)
                .Select(s => s.Userdata).FirstOrDefault();
            }
            // if (_result.UserData is null)
            // {
            //     throw new WebServer.Exception("There are no sake data found in the database, please manually add it in the database");
            // }
        }
        protected override void ResponseConstruct()
        {
            _response = new SearchForRecordResponse(_request, _result);
        }
    }
}
