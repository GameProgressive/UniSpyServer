using System;
using UniSpy.Server.PresenceConnectionManager.Abstraction.BaseClass;
using UniSpy.Server.PresenceConnectionManager.Contract.Request;
using UniSpy.Server.PresenceConnectionManager.Contract.Result;

namespace UniSpy.Server.PresenceConnectionManager.Contract.Response
{
    public sealed class AddBuddyResponse : ResponseBase
    {
        private new AddBuddyRequest _request => (AddBuddyRequest)base._request;
        private new AddBuddyResult _result => (AddBuddyResult)base._result;
        public AddBuddyResponse(AddBuddyRequest request, AddBuddyResult result) : base(request, result)
        {
        }

        public override void Build()
        {
            // \bm\<buddy message type>\f\<profile id>\date\<date>
            // GPI_BM_MESSAGE: \msg\<msg>\
            // GPI_BM_UTM:\msg\<msg>\
            // GPI_BM_REQUEST:\msg\|signed|<signed data>\
            // GPI_BM_AUTH:
            // GPI_BM_REVOKE:
            // GPI_BM_STATUS:\msg\|s|<status code>\ or \msg\|ss|<status info status string>|ls|<location string>|ip|<ip>|p|<product id>|qm|<quiet mode flag>
            // GPI_BM_INVITE:\msg\|p|<product id>|l|<location string>
            // GPI_BM_PING:\msg\\
            throw new NotImplementedException();
        }
    }
}
