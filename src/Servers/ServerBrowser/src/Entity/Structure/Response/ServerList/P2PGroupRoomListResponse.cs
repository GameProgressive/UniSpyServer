using ServerBrowser.Abstraction.BaseClass;
using ServerBrowser.Entity.Enumerate;
using ServerBrowser.Entity.Structure.Misc;
using ServerBrowser.Entity.Structure.Request;
using ServerBrowser.Entity.Structure.Result;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Encryption;
using UniSpyLib.Extensions;

namespace ServerBrowser.Entity.Structure.Response.ServerList
{
    public sealed class P2PGroupRoomListResponse : ServerListUpdateOptionResponseBase
    {
        private new P2PGroupRoomListResult _result => (P2PGroupRoomListResult)base._result;
        private new ServerListRequest _request => (ServerListRequest)base._request;
        public P2PGroupRoomListResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }
        public override void Build()
        {
            base.Build();
            SendingBuffer = _serverListData.ToArray();
        }

        protected override void BuildServersInfo()
        {
            foreach (var room in _result.PeerGroupInfo.PeerRooms)
            {
                //add has key flag
                _serverListData.Add((byte)GameServerFlags.HasKeysFlag);
                //in group list server ip is group id

                byte[] groupid = ByteTools.GetBytes(room.GroupID, true);

                _serverListData.AddRange(groupid);

                foreach (var key in _request.Keys)
                {
                    _serverListData.Add(StringFlag.NTSStringFlag);
                    var value = room.GetValuebyGameSpyDefinedName(key);
                    _serverListData.AddRange(UniSpyEncoding.GetBytes(value));
                    _serverListData.Add(StringFlag.StringSpliter);
                }
            }
        }
    }
}
