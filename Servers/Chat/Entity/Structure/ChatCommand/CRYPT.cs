using System;
using Chat.Entity.Structure.Enumerator.Request;

namespace Chat.Entity.Structure.ChatCommand
{
    public class CRYPT : ChatCommandBase
    {
        public string VersionID { get; protected set; }
        public string GameName { get; protected set; }
        //CRYPT des %d %s
        public CRYPT(string request) : base(request)
        {
            VersionID = _requestFrag[2];
            GameName = _requestFrag[3];
        }

        public CRYPT() : base(ChatRequest.CRYPT)
        { }

        public override string BuildCommandString(params string[] param)
        {
            throw new NotImplementedException();
        }
    }
}
