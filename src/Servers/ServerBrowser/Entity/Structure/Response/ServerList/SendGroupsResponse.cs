using ServerBrowser.Abstraction.BaseClass;
using ServerBrowser.Entity.Enumerate;
using ServerBrowser.Entity.Structure.Misc;
using ServerBrowser.Entity.Structure.Request;
using ServerBrowser.Entity.Structure.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        protected override List<byte> BuildServersInfo()
        {
            List<byte> data = new List<byte>();
            foreach (var room in _result.PeerGroupInfo.PeerRooms)
            {
                //add has key flag
                data.Add((byte)GameServerFlags.HasKeysFlag);
                //in group list server ip is group id

                byte[] groupid = ByteTools.GetBytes(int.Parse(room.KeyValue["groupid"]), true);

                data.AddRange(groupid);

                foreach (var key in _request.Keys)
                {
                    data.Add(SBStringFlag.NTSStringFlag);
                    data.AddRange(Encoding.ASCII.GetBytes(room.KeyValue[key]));
                    data.Add(SBStringFlag.StringSpliter);
                }
            }
            return data;
        }
    }
}
