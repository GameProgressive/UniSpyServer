using System;
namespace Chat.Entity.Structure.ChatCommand.ChatBasic
{
    public class REGISTERNICK : ChatCommandBase
    {
        public string NamespaceID { get; protected set; }
        public string UniqueNick { get; protected set; }
        public string CDKey { get; protected set; }
        public override bool Parse()
        {
            bool flag = base.Parse();
            NamespaceID = _cmdParameters[0];
            UniqueNick = _cmdParameters[1];
            CDKey = _cmdParameters[2];
            return flag;
        }
    }
}
