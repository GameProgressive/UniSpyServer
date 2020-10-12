using System;
using Chat.Entity.Structure.ChatResponse;

namespace Chat.Entity.Structure.ChatCommand
{
    public class CRYPTRequest : ChatRequestBase
    {
        public CRYPTRequest(string rawRequest) : base(rawRequest)
        {
        }

        public string VersionID { get; protected set; }
        public string GameName { get; protected set; }
        //CRYPT des %d %s

        protected override bool DetailParse()
        {
           

            VersionID = _cmdParams[1];
            GameName = _cmdParams[2];

            return true;
        }


    }
}
