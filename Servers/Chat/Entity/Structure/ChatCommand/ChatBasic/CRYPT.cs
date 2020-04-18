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
        }

        public CRYPT() : base()
        { }
        

        public override bool Parse()
        {
           bool flag = base.Parse();

            VersionID = _cmdParameters[1];
            GameName = _cmdParameters[2];

            return flag;
        }

        public override string BuildCommandString(params string[] param)
        {
            throw new NotImplementedException();
        }
    }
}
