namespace Chat.Entity.Structure.ChatCommand
{
    public class LIST : ChatCommandBase
    {
        public string Filter { get; protected set; }

        public override bool Parse(string request)
        {
            if (!base.Parse(request))
            {
                return false;
            }
            Filter = _cmdParams[0];
            return true;
        }
    }
}
