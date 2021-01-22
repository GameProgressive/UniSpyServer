using QueryReport.Entity.Structure.Group;
using ServerBrowser.Abstraction.BaseClass;
using ServerBrowser.Entity.Enumerate;
using ServerBrowser.Entity.Structure;
using ServerBrowser.Entity.Structure.Misc;
using System.Text;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Extensions;

namespace ServerBrowser.Handler.CmdHandler
{
    internal sealed class SendGroupsHandler : UpdateOptionHandlerBase
    {
        private PeerGroupInfo _peerGroup;

        public SendGroupsHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        protected override void DataOperation()
        {
            base.DataOperation();
            var matchedKey = PeerGroupInfo.RedisOperator.GetMatchedKeys(_request.GameName);
            // Game name is unique in redis database
            if (matchedKey.Count != 1)
            {
                _errorCode = SBErrorCode.NoGroupRoomFound;
                return;
            }

            _peerGroup = PeerGroupInfo.RedisOperator.GetSpecificValue(matchedKey[0]);
        }

        protected override void ResponseConstruct()
        {
            base.ResponseConstruct();
            GenerateServerKeys();
            GenerateUniqueValue();
            GenerateServersInfo();
            _dataList.AddRange(SBStringFlag.AllServerEndFlag);
        }

        protected override void GenerateServersInfo()
        {

            foreach (var room in _peerGroup.PeerRooms)
            {
                //add has key flag
                _dataList.Add((byte)GameServerFlags.HasKeysFlag);
                //in group list server ip is group id

                byte[] groupid = ByteTools.GetBytes(int.Parse(room.KeyValue["groupid"]), true);

                _dataList.AddRange(groupid);

                foreach (var key in _request.Keys)
                {
                    _dataList.Add(SBStringFlag.NTSStringFlag);
                    _dataList.AddRange(Encoding.ASCII.GetBytes(room.KeyValue[key]));
                    _dataList.Add(SBStringFlag.StringSpliter);
                }
            }
        }
    }
}
