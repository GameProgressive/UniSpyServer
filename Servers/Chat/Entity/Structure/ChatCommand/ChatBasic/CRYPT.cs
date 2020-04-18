using System;

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
            if (!base.Parse())
            {
                return false;
            }

            VersionID = _cmdParams[1];
            GameName = _cmdParams[2];

            return true;
        }

        public override string BuildCommandString(params string[] param)
        {
            throw new NotImplementedException();
        }
    }
}
