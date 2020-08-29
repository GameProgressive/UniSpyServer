using System;
using Chat.Entity.Structure.ChatResponse;

namespace Chat.Entity.Structure.ChatCommand
{
    public class CRYPT : ChatRequestBase
    {
        public CRYPT(string rawRequest) : base(rawRequest)
        {
        }

        public string VersionID { get; protected set; }
        public string GameName { get; protected set; }
        //CRYPT des %d %s

        public override bool Parse()
        {
            if (!Parse())
            {
                return false;
            }

            VersionID = _cmdParams[1];
            GameName = _cmdParams[2];

            return true;
        }


    }
}
