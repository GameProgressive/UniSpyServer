using System;
using Chat.Entity.Structure.ChatResponse;

namespace Chat.Entity.Structure.ChatCommand
{
    public class CRYPT : ChatCommandBase
    {

        public string VersionID { get; protected set; }
        public string GameName { get; protected set; }
        //CRYPT des %d %s

        public override bool Parse(string recv)
        {
            if (!base.Parse(recv))
            {
                return false;
            }

            VersionID = _cmdParams[1];
            GameName = _cmdParams[2];

            return true;
        }


    }
}
