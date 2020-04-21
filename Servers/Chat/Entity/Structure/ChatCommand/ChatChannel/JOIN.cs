using Chat.Server;

namespace Chat.Entity.Structure.ChatCommand
{
    public class JOIN : ChatChannelCommandBase
    {

        public string Password { get; protected set; }

        public override bool Parse(string request)
        {
            if (!base.Parse(request))
            { return false; }
            if (_cmdParams.Count > 2)
            {
                return false;
            }
            if (_cmdParams.Count == 2)
            {
                Password = _cmdParams[1];
            }
            return true;
        }
    }
}
