using Chat.Abstraction.BaseClass;
using Chat.Entity.Exception;

namespace Chat.Entity.Structure.Request
{
    internal sealed class KICKRequest : ChatChannelRequestBase
    {
        public new string ChannelName
        {
            get => base.ChannelName;
            set => base.ChannelName = value;
        }
        public KICKRequest() { }
        public KICKRequest(string rawRequest) : base(rawRequest)
        {
        }
        public string KickeeNickName { get; set; }
        public string Reason { get; set; }

        public override void Parse()
        {
            base.Parse();

            if (_cmdParams.Count != 2)
            {
                throw new ChatException("number of IRC parameters are incorrect.");
            }
            KickeeNickName = _cmdParams[1];
            if (_longParam == null)
            {
                throw new ChatException("IRC long parameters is missing.");
            }
            Reason = _longParam;
        }
    }
}
