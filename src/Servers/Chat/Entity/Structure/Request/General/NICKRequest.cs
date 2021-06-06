using Chat.Abstraction.BaseClass;

namespace Chat.Entity.Structure.Request.General
{
    public class NICKRequest : ChatRequestBase
    {
        public NICKRequest(string rawRequest) : base(rawRequest)
        {
        }

        public string NickName { get; protected set; }

        public override void Parse()
        {
            base.Parse();


            if (_cmdParams.Count == 0)
            {
                NickName = _longParam;
            }
            else
            {
                NickName = _cmdParams[0];
            }
        }
    }
}
