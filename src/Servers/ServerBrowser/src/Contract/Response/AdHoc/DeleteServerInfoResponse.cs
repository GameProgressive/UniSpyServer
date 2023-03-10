using UniSpy.Server.ServerBrowser.Abstraction.BaseClass;
using UniSpy.Server.ServerBrowser.Contract.Result;
using UniSpy.Server.ServerBrowser.Enumerate;

namespace UniSpy.Server.ServerBrowser.Contract.Response.AdHoc
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