using System;
namespace Chat.Entity.Structure.ChatCommand.ChatBasic
{
    public class USER : ChatCommandBase
    {
        public USER()
        {
        }

        public USER(string request) : base(request)
        {
        }

        public string UserName { get; protected set; }
        public string IPAddress { get; protected set; }
        public string ServerName { get; protected set; }
        public string NickName { get; protected set; }
        public override bool Parse()
        {
            bool flag = base.Parse();

            UserName = _cmdParams[0];
            IPAddress = _cmdParams[1];
            ServerName = _cmdParams[2];
            NickName = _cmdParams[3];
            return flag;
        }
    }
}
