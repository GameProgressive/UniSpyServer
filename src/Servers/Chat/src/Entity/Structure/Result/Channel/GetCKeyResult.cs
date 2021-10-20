using UniSpyServer.Chat.Abstraction.BaseClass;
using UniSpyServer.Chat.Entity.Contract;
using System.Collections.Generic;

namespace UniSpyServer.Chat.Entity.Structure.Result.Channel
{
    public sealed class GETCKEYDataModel
    {
        public string NickName { get; set; }
        public string UserValues { get; set; }
    }
    public sealed class GetCKeyResult : ResultBase
    {
        public List<GETCKEYDataModel> DataResults { get; }
        public string ChannelName { get; set; }
        public GetCKeyResult()
        {
            DataResults = new List<GETCKEYDataModel>();
        }
    }
}
