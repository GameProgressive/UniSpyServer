using System;
namespace Chat.Entity.Structure.ChatCommand.ChatChannel
{
    public class KICK:ChatChannelCommandBase
    {
        public KICK(string request) : base(request)
        {
        }

        public string UserName { get; protected set; }
        public string Reason { get; protected set; }

        public override bool Parse()
        {
            if (!base.Parse())
                return false;
            if(_cmdParams.Count!=1)
            {
                return false;
            }
            UserName = _cmdParams[0];
            if (_longParam == null)
                return false;
            Reason = _longParam;
            return true;
        }
    }
}
