using System;
namespace Chat.Entity.Structure.ChatCommand
{
    public class QUIT : ChatCommandBase
    {
        public string Reason { get; protected set; }

        public override bool Parse(string request)
        {
            base.Parse(request);

            if (_longParam == null)
            {
                return false;
            }

            Reason = _longParam;
            return true;
        }
    }
}
