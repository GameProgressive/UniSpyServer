using QueryReport.Entity.Structure.Redis;
using ServerBrowser.Abstraction.BaseClass;
using ServerBrowser.Entity.Enumerate;
using ServerBrowser.Entity.Exception;
using ServerBrowser.Entity.Structure.Response.ServerList;
using ServerBrowser.Entity.Structure.Result;
using UniSpyLib.Abstraction.Interface;
namespace ServerBrowser.Handler.CmdHandler
{
    internal sealed class P2PGroupRoomListHandler : ServerListUpdateOptionHandlerBase
    {
        private PeerGroupInfo _peerGroup;
        public P2PGroupRoomListHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _result = new P2PGroupRoomListResult();
        }

        protected override void DataOperation()
        {
            var searchKey = new PeerGroupInfoRedisKey()
            {
                GameName = _request.GameName
            };
            var matchedKey = PeerGroupInfoRedisOperator.GetMatchedKeys(searchKey);

            // Game name is unique in redis database
            if (matchedKey.Count != 1)
            {
                throw new SBException("There are no group room found in database.");
            }

            _peerGroup = PeerGroupInfoRedisOperator.GetSpecificValue(matchedKey[0]);
        }

        protected override void ResponseConstruct()
        {
            _response = new P2PGroupRoomListResponse(_request, _result);
        }
    }
}
