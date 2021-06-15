using Chat.Abstraction.BaseClass;

namespace Chat.Entity.Structure.Request.General
{
    internal sealed class USERRequest : ChatRequestBase
    {
        public USERRequest(string rawRequest) : base(rawRequest)
        {
        }

        public string UserName { get; private set; }
        public string Hostname { get; private set; }
        public string ServerName { get; private set; }
        public string NickName { get; private set; }
        public string Name { get; private set; }

        public override void Parse()
        {
            base.Parse();

            UserName = _cmdParams[0];
            Hostname = _cmdParams[1];
            ServerName = _cmdParams[2];
            Name = _longParam;
        }
    }
}
