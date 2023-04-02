using System;
using System.Linq;
using UniSpy.Server.ServerBrowser.V2.Abstraction.BaseClass;
using UniSpy.Server.ServerBrowser.V2.Enumerate;
using UniSpy.Server.ServerBrowser.V2.Aggregate.Misc;
using UniSpy.Server.ServerBrowser.V2.Contract.Request;
using UniSpy.Server.ServerBrowser.V2.Contract.Result;
using UniSpy.Server.Core.Encryption;

namespace UniSpy.Server.ServerBrowser.V2.Contract.Response.ServerList
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
            BuildServersFullInfo();
            SendingBuffer = _serversInfoBuffer.ToArray();
        }

        protected override void BuildServersFullInfo()
        {
            foreach (var room in _result.PeerGroupInfo.PeerRooms)
            {
                //add has key flag
                _serversInfoBuffer.Add((byte)GameServerFlags.HasKeysFlag);
                //in group list server ip is group id
                // group id = 0 means the end flag of group list
                var groupId = 0;
                if (room != _result.PeerGroupInfo.PeerRooms.Last())
                {
                    groupId = room.GroupId;
                }
                var groupIdBytes = BitConverter.GetBytes(groupId).Reverse().ToArray();
                _serversInfoBuffer.AddRange(groupIdBytes);

                foreach (var key in _request.Keys)
                {
                    _serversInfoBuffer.Add(StringFlag.NTSStringFlag);
                    var value = room.KeyValues.ContainsKey(key) ? room.KeyValues[key] : "";
                    // if key is uint or int, we need first convert to ASCII string then get bytes
                    _serversInfoBuffer.AddRange(UniSpyEncoding.GetBytes(value.ToString()));
                    _serversInfoBuffer.Add(StringFlag.StringSpliter);
                }
            }
        }
    }
}
