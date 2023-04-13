using System;
using UniSpy.Server.ServerBrowser.V1.Abstraction.BaseClass;
using UniSpy.Server.ServerBrowser.V1.Contract.Request;
using UniSpy.Server.ServerBrowser.V1.Contract.Result;

namespace UniSpy.Server.ServerBrowser.V1.Contract.Response
{
    public sealed class ListResponse : ResponseBase
    {
        private new ListRequest _request => (ListRequest)base._request;
        private new ListResult _result => (ListResult)base._result;
        public ListResponse(ListRequest request, ListResult result) : base(request, result)
        {
        }

        public override void Build()
        {
            SendingBuffer = $@"\fieldcount\{_result.ServersInfo.Count}";
            if (_request.IsSendAllInfo)
            {
                BuildServerAllInfo();
            }
            else
            {
                BuildServerGeneralInfo();
            }

            SendingBuffer += @"\final\";
        }


        public void BuildServerAllInfo()
        {

        }
        public void BuildServerGeneralInfo()
        {
            foreach (var info in _result.ServersInfo)
            {
                if (_request.IsSendingCompressFormat)
                {
                    var buffer = BitConverter.ToString(info.HostIPAddress.GetAddressBytes()) + BitConverter.ToString(BitConverter.GetBytes((short)info.HostPort));
                    SendingBuffer += @$"\ip\{buffer}";
                }
                SendingBuffer += @$"\ip\{info.HostIPEndPoint}";
            }
        }
    }
}