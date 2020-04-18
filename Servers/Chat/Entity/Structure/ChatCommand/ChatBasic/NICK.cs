using System;
namespace Chat.Entity.Structure.ChatCommand.ChatBasic
{
    public class NICK:ChatCommandBase
    {
        public string NickName { get; protected set; }
        public override bool Parse()
        {
            bool flag = base.Parse();
            NickName = LongParameter;
            return flag;
        }
    }
}
