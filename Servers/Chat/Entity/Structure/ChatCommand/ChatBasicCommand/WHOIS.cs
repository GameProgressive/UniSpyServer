namespace Chat.Entity.Structure.ChatCommand
{
    public class WHOIS : ChatCommandBase
    {
        public string NickName { get; protected set; }

        public override bool Parse(string recv)
        {
            if (!base.Parse(recv))
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
