using System;
using UniSpy.Server.ServerBrowser.V1.Abstraction.BaseClass;
using UniSpy.Server.ServerBrowser.V1.Contract.Request;
using UniSpy.Server.ServerBrowser.V1.Contract.Result;

namespace UniSpy.Server.ServerBrowser.V1.Contract.Response
{
    public sealed class ListResponse : ResponseBase
    {
        public new ListRequest _request => (ListRequest)base._request;
        public new ListResult _result => (ListResult)base._result;
        public ListResponse(ListRequest request, ListResult result) : base(request, result)
        {
        }

        public override void Build()
        {
            SendingBuffer = $@"\fieldcount\{_result.ServerIPList.Count}";
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
            foreach (var endPoint in _result.ServerIPList)
            {
                if (_request.IsSendingCompressFormat)
                {
                    var buffer = BitConverter.ToString(endPoint.Address.GetAddressBytes()) + BitConverter.ToString(BitConverter.GetBytes((short)endPoint.Port));
                    SendingBuffer += @$"\ip\{buffer}";
                }
                SendingBuffer += @$"\ip\{endPoint}";
            }
        }
    }
}