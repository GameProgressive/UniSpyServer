using GameSpyLib.Abstraction.Interface;
using GameSpyLib.Extensions;
using QueryReport.Entity.Structure.Group;
using ServerBrowser.Entity.Enumerator;
using ServerBrowser.Entity.Structure;
using ServerBrowser.Entity.Structure.Packet.Request;
using System.Text;

namespace ServerBrowser.Handler.CommandHandler.ServerList.UpdateOptionHandler.SendGroups
{
    public class SendGroupsHandler : UpdateOptionHandlerBase
    {
        private PeerGroup _peerGroup;

        public SendGroupsHandler(ServerListRequest request, ISession client, byte[] recv) : base(request, client, recv)
        {
        }

        protected override void DataOperation()
        {
            base.DataOperation();
            _peerGroup = PeerGroup.GetGroupsList(_request.GameName);
            if (_peerGroup == null || _peerGroup.PeerRooms.Count == 0)
            {
                _errorCode = SBErrorCode.NoGroupRoomFound;
                return;
            }
        }

        protected override void ConstructResponse()
        {
            base.ConstructResponse();
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
