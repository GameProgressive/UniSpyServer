namespace Chat.Entity.Structure.ChatCommand.ChatBasic
{
    public class LIST : ChatCommandBase
    {
        public string Filter { get; protected set; }

        public LIST()
        {
        }

        public LIST(string request) : base(request)
        {
        }

        public override bool Parse()
        {
            if (!base.Parse())
            {
                return false;
            }
            Filter = _cmdParams[0];
            return true;
        }
    }
}
