using System;
namespace Chat.Entity.Structure.ChatCommand
{
    public class QUIT : ChatCommandBase
    {
        public string Reason { get; protected set; }

        public override bool Parse(string recv)
        {
            base.Parse(recv);

            if (_longParam == null)
            {
                return false;
            }

            Reason = _longParam;
            return true;
        }
    }
}
