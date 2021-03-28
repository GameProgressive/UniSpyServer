using ServerBrowser.Abstraction.BaseClass;
using ServerBrowser.Entity.Enumerate;
using ServerBrowser.Entity.Structure.Misc;
using ServerBrowser.Entity.Structure.Request;
using ServerBrowser.Entity.Structure.Result;
using System.Text;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Extensions;

namespace ServerBrowser.Entity.Structure.Response.ServerList
{
    internal sealed class SendGroupsResponse : ServerListResponseBase
    {
        private new SendGroupsResult _result => (SendGroupsResult)base._result;
        private new ServerListRequest _request => (ServerListRequest)base._request;
        public SendGroupsResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

        protected override void BuildServersInfo()
        {
            foreach (var room in _result.PeerGroupInfo.PeerRooms)
            {
                //add has key flag
                _serverListContext.Add((byte)GameServerFlags.HasKeysFlag);
                //in group list server ip is group id

                byte[] groupid = ByteTools.GetBytes(room.GroupID, true);

                _serverListContext.AddRange(groupid);

                foreach (var key in _request.Keys)
                {
                    _serverListContext.Add(SBStringFlag.NTSStringFlag);
                    var value = room.GetValuebyGameSpyDefinedName(key);
                    _serverListContext.AddRange(Encoding.ASCII.GetBytes(value));
                    _serverListContext.Add(SBStringFlag.StringSpliter);
                }
            }
        }
    }
}
