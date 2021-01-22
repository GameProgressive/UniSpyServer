using QueryReport.Entity.Structure;
using ServerBrowser.Entity.Enumerate;
using ServerBrowser.Entity.Structure.Result;
using System.Collections.Generic;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Extensions;

namespace ServerBrowser.Abstraction.BaseClass
{
    internal abstract class UpdateOptionHandlerBase : SBCmdHandlerBase
    {
        protected byte[] _clientRemoteIP { get; set; }
        protected byte[] _gameServerDefaultHostPort { get; set; }
        protected string _secretKey;
        protected new UpdateOptionRequestBase _request => (UpdateOptionRequestBase)base._request;
        protected new ServerListResult _result
        {
            get => (ServerListResult)base._result;
            set => base._result = value;
        }
        protected List<byte> _dataList { get; set; }
        protected List<GameServerInfo> _gameServers { get; set; }
        public UpdateOptionHandlerBase(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }
        protected override void RequestCheck()
        {
            //we first check and get secrete key from database
            if (!DataOperationExtensions
                .GetSecretKey(_request.GameName, out _secretKey))
            {
                _result.ErrorCode = SBErrorCode.UnSupportedGame;
                return;
            }
            //this is client public ip and default query port
            _result.ClientRemoteIP = _session.RemoteIPEndPoint.Address.GetAddressBytes();
        }

    }
}
