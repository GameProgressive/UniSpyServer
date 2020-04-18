using System;
namespace Chat.Entity.Structure.ChatCommand.ChatChannel
{
    public class JOIN : ChatChannelCommandBase
    {

        public string Password { get; protected set; }
        public override bool Parse()
        {
            bool flag = base.Parse();
            if (_cmdParams.Count != 2)
            {
                return false;
            }
            Password = _cmdParams[1];
            return flag;
        }
    }
}
