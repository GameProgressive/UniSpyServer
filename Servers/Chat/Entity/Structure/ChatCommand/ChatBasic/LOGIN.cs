using System;
using System.Linq;

namespace Chat.Entity.Structure.ChatCommand.ChatBasic
{
    public class LOGIN:ChatCommandBase
    {
        public string NameSpaceID { get; protected set; }
        public string ProfileNick { get; protected set; }
        public string Email { get; protected set; }
        public string UniqueNick { get; protected set; }
        public string PasswordHash { get; protected set; }
        public bool IsUniqueNickLogin { get; protected set; }

        public override bool Parse()
        {
           bool frag = base.Parse();

            NameSpaceID = _cmdParameters[0];
            if (_cmdParameters.Contains("*"))
            {
                IsUniqueNickLogin = false;
                PasswordHash = _cmdParameters[1];

                if (LongParameter.Where(c => c.Equals("@")).Count() != 2)
                {
                    return false;
                }
                int profilenickIndex = LongParameter.IndexOf("@");

                ProfileNick = LongParameter.Substring(1, profilenickIndex - 1);
                Email = LongParameter.Substring(profilenickIndex);
            }
            else
            {
                UniqueNick = _cmdParameters[1];
                PasswordHash = _cmdParameters[2];
            }

            return true;
        }
    }
}
