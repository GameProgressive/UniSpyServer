using System;

namespace Chat.Entity.Structure.ChatCommand
{
    public class CRYPT : ChatCommandBase
    {
        public string VersionID { get; protected set; }
        public string GameName { get; protected set; }
        //CRYPT des %d %s

        public override bool Parse(string request)
        {
            if (!base.Parse(request))
            {
                return false;
            }

            VersionID = _cmdParams[1];
            GameName = _cmdParams[2];

            return true;
        }

        public override string BuildRPL(params string[] param)
        {
            throw new NotImplementedException();
        }
    }
}
