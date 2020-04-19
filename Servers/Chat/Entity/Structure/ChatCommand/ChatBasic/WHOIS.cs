namespace Chat.Entity.Structure.ChatCommand
{
    public class WHOIS : ChatCommandBase
    {
        public string UserName { get; protected set; }

        public override bool Parse(string request)
        {
            if (!base.Parse(request))
            { return false; }
            if (_cmdParams.Count != 1)
            {
                return false;
            }

            UserName = _cmdParams[0];
            return true;
        }
    }
}
