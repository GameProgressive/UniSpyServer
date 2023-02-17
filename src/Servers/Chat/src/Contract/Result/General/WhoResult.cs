using UniSpy.Server.Chat.Abstraction.BaseClass;
using System.Collections.Generic;

namespace UniSpy.Server.Chat.Contract.Result.General
{
    public sealed class WhoDataModel
    {
        public string ChannelName { get; set; }
        public string UserName { get; set; }
        public string PublicIPAddress { get; set; }
        public string NickName { get; set; }
        public string Modes { get; set; }
    }
    public sealed class WhoResult : ResultBase
    {
        public List<WhoDataModel> DataModels { get; }
        public WhoResult()
        {
            DataModels = new List<WhoDataModel>();
        }
    }
}
