﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniSpyServer.Servers.WebServer.Abstraction;
using UniSpyServer.Servers.WebServer.Entity.Contract;
using UniSpyServer.Servers.WebServer.Module.D2G.Entity.Structure.Request;
using UniSpyServer.Servers.WebServer.Module.D2G.Entity.Structure.Response;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.WebServer.Module.D2G.Handler
{
    [HandlerContract("GetPurchaseHistory")]
    internal class GetPurchaseHistoryHandler : CmdHandlerBase
    {
        protected new GetPurchaseHistoryRequest _request => (GetPurchaseHistoryRequest)base._request;
        //protected new GetPurchaseHistoryResponse _result = new GetPurchaseHistoryResponse();

        public GetPurchaseHistoryHandler(IClient client, IRequest request) : base(client, request)
        {

        }

        protected override void DataOperation()
        {

        }

        protected override void ResponseConstruct()
        {
            _response = new GetPurchaseHistoryResponse(_request, null);
        }
    }
}
