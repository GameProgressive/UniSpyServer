using QueryReport.Entity.Structure.Group;
using ServerBrowser.Abstraction.BaseClass;
using ServerBrowser.Entity.Enumerate;
using ServerBrowser.Entity.Structure.Misc;
using ServerBrowser.Entity.Structure.Response.ServerList;
using ServerBrowser.Entity.Structure.Result;
using UniSpyLib.Abstraction.Interface;
using QueryReport.Handler.SystemHandler.Redis;
namespace ServerBrowser.Handler.CmdHandler
{
    internal sealed class SendGroupsHandler : ServerListHandlerBase
    {
        private PeerGroupInfo _peerGroup;

        public SendGroupsHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _result = new SendGroupsResult();
        }

        protected override void DataOperation()
        {
            var matchedKey = PeerGroupInfoRedisOperator.GetMatchedKeys(_request.GameName);
            // Game name is unique in redis database
            if (matchedKey.Count != 1)
            {
                _result.ErrorCode = SBErrorCode.NoGroupRoomFound;
                return;
            }

            _peerGroup = PeerGroupInfoRedisOperator.GetSpecificValue(matchedKey[0]);
        }

        protected override void ResponseConstruct()
        {
            _response = new SendGroupsResponse(_request, _result);
        }

    }
}
