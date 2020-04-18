using System;
namespace Chat.Entity.Structure.ChatCommand.ChatBasic
{
    public class USER : ChatCommandBase
    {
        public string UserName { get; protected set; }
        public string IPAddress { get; protected set; }
        public string ServerName { get; protected set; }
        public string NickName { get; protected set; }
        public override bool Parse()
        {
            bool flag = base.Parse();

            UserName = _cmdParameters[0];
            IPAddress = _cmdParameters[1];
            ServerName = _cmdParameters[2];
            NickName = _cmdParameters[3];
            return flag;
        }
    }
}
