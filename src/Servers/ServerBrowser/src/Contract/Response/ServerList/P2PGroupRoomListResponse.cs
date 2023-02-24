using System;
using System.Linq;
using UniSpy.Server.ServerBrowser.Abstraction.BaseClass;
using UniSpy.Server.ServerBrowser.Enumerate;
using UniSpy.Server.ServerBrowser.Aggregate.Misc;
using UniSpy.Server.ServerBrowser.Contract.Request;
using UniSpy.Server.ServerBrowser.Contract.Result;
using UniSpy.Server.Core.Encryption;

namespace UniSpy.Server.ServerBrowser.Contract.Response.ServerList
{
    public sealed class P2PGroupRoomListResponse : ServerListUpdateOptionResponseBase
    {
        private new ServerListRequest _request => (ServerListRequest)base._request;
        private new P2PGroupRoomListResult _result => (P2PGroupRoomListResult)base._result;
        public P2PGroupRoomListResponse(ServerListUpdateOptionRequestBase request, ServerListUpdateOptionResultBase result) : base(request, result)
        {
        }
        public override void Build()
        {
            base.Build();
            // we add the server keys
            BuildServerKeys();
            //we use NTS string so total unique value list is 0
            BuildUniqueValue();
            //add server infomation such as public ip etc.
            BuildServerFullInfo();
            SendingBuffer = _serverListData.ToArray();
        }

        protected override void BuildServerFullInfo()
        {
            foreach (var room in _result.PeerGroupInfo.PeerRooms)
            {
                //add has key flag
                _serverListData.Add((byte)GameServerFlags.HasKeysFlag);
                //in group list server ip is group id
                // group id = 0 means the end flag of group list
                var groupId = 0;
                if (room != _result.PeerGroupInfo.PeerRooms.Last())
                {
                    groupId = room.GroupId;
                }
                var groupIdBytes = BitConverter.GetBytes(groupId).Reverse().ToArray();
                _serverListData.AddRange(groupIdBytes);

                foreach (var key in _request.Keys)
                {
                    _serverListData.Add(StringFlag.NTSStringFlag);
                    var value = room.KeyValues.ContainsKey(key) ? room.KeyValues[key] : "";
                    // if key is uint or int, we need first convert to ASCII string then get bytes
                    _serverListData.AddRange(UniSpyEncoding.GetBytes(value.ToString()));
                    _serverListData.Add(StringFlag.StringSpliter);
                }
            }
        }
    }
}
