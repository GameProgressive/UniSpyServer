using System.Text.RegularExpressions;
using System.Linq;
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
            switch (_request.Type)
            {
                case ListRequestType.Basic:
                    BuildServerGeneralInfo();
                    break;
                case ListRequestType.Info:
                    BuildServerAllInfo();
                    break;
                case ListRequestType.Group:
                    BuildGroupInfo();
                    break;
            }

            SendingBuffer += @"\final\";
        }
        /// <summary>
        /// Build group infos
        /// </summary>
        public void BuildGroupInfo()
        {
            SendingBuffer = @"\group" + SendingBuffer;
            foreach (var group in _result.PeerRoomsInfo)
            {
                foreach (var kv in group.KeyValues)
                {
                    SendingBuffer += @$"\{kv.Key}\{kv.Value}";
                }
            }
        }
        /// <summary>
        /// Build servers detailed info (contain all keyvalues of all server)
        /// </summary>
        public void BuildServerAllInfo()
        {
            SendingBuffer = @"\list2" + SendingBuffer;
            foreach (var info in _result.ServersInfo)
            {
                foreach (var kv in info.KeyValues)
                {
                    SendingBuffer += @$"\{kv.Key}\{kv.Value}";
                }
            }
        }

        /// <summary>
        /// Build general info for servers (ip and port)
        /// </summary>
        public void BuildServerGeneralInfo()
        {
            SendingBuffer = @"\list2" + SendingBuffer;

            foreach (var info in _result.ServersInfo)
            {
                if (_request.IsSendingCompressFormat)
                {
                    var portBytes = BitConverter.GetBytes((short)info.HostPort);
                    var buffer = BitConverter.ToString(info.HostIPAddress.GetAddressBytes()) + BitConverter.ToString(portBytes);
                    SendingBuffer += buffer;
                }
                else
                {
                    SendingBuffer += @$"\ip\{info.HostIPEndPoint}";
                }
            }
        }
    }
}