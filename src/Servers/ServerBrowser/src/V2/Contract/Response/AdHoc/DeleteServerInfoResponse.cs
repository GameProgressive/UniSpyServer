using UniSpy.Server.ServerBrowser.V2.Abstraction.BaseClass;
using UniSpy.Server.ServerBrowser.V2.Contract.Result;
using UniSpy.Server.ServerBrowser.V2.Enumerate;

namespace UniSpy.Server.ServerBrowser.V2.Contract.Response.AdHoc
{
    public sealed class DeleteServerInfoResponse : AdHocResponseBase
    {
        public DeleteServerInfoResponse(AdHocResult result) : base(result)
        {
        }

        public override void Build()
        {
            _buffer.Add((byte)ResponseType.DeleteServerMessage);
            _buffer.AddRange(_result.GameServerInfo.HostIPAddress.GetAddressBytes());
            _buffer.AddRange(_result.GameServerInfo.QueryReportPortBytes);
            SendingBuffer = _buffer.ToArray();
        }
    }
}