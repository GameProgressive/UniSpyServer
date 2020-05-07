namespace Chat.Entity.Structure.ChatCommand
{
    public class CDKEY : ChatCommandBase
    {
        public string CDKey { get; protected set; }
        
        public override bool Parse(string request)
        {
            if (!base.Parse(request))
            {
                return false;
            }
            CDKey = _cmdParams[0];
            return true;
        }
    }
}
