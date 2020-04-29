using System;
namespace Chat.Entity.Structure.ChatCommand
{
    public class QUIT : ChatCommandBase
    {
        public string Reason { get; protected set; }

        public override bool Parse(string request)
        {
            if( base.Parse(request))
            {
                return false;
            }
            if (_longParam == null)
            {
                return false;
            }

            Reason = _longParam;
            return true;
        }
    }
}
