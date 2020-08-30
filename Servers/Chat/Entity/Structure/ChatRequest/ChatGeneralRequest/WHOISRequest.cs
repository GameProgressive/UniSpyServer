namespace Chat.Entity.Structure.ChatCommand
{
    public class WHOISRequest : ChatRequestBase
    {
        public WHOISRequest(string rawRequest) : base(rawRequest)
        {
        }

        public string NickName { get; protected set; }

        public override bool Parse()
        {
            if (!base.Parse())
            { return false; }
            if (_cmdParams.Count != 1)
            {
                return false;
            }

            NickName = _cmdParams[0];
            return true;
        }
    }
}
