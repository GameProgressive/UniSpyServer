using System;
namespace Chat.Entity.Structure.ChatCommand
{
    public class QUIT : ChatRequestBase
    {
        public QUIT(string rawRequest) : base(rawRequest)
        {
        }

        public string Reason { get; protected set; }

        public override bool Parse()
        {
            if(!base.Parse())
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
