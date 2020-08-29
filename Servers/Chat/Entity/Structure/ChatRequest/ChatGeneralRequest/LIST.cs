namespace Chat.Entity.Structure.ChatCommand
{
    public class LIST : ChatRequestBase
    {
        public LIST(string rawRequest) : base(rawRequest)
        {
        }

        public bool IsSearchingChannel { get; protected set; }
        public bool IsSearchingUser { get; protected set; }
        public string Filter { get; protected set; }

        public override bool Parse()
        {
            if (!Parse())
            {
                return false;
            }
            if (_cmdParams.Count == 0)
            {
                IsSearchingChannel = true;
                return true;
            }

            Filter = _cmdParams[0];
            return true;
        }

    }
}
