namespace Chat.Entity.Structure.ChatCommand
{
    public class LISTRequest : ChatRequestBase
    {
        public LISTRequest(string rawRequest) : base(rawRequest)
        {
        }

        public bool IsSearchingChannel { get; protected set; }
        public bool IsSearchingUser { get; protected set; }
        public string Filter { get; protected set; }

        protected override bool DetailParse()
        {
           
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
