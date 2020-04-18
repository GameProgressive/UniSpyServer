using System.Linq;

namespace Chat.Entity.Structure.ChatCommand.ChatBasic
{
    public class LOGIN : ChatCommandBase
    {
        public LOGIN()
        {
        }

        public LOGIN(string request) : base(request)
        {
        }

        public string NameSpaceID { get; protected set; }
        public string ProfileNick { get; protected set; }
        public string Email { get; protected set; }
        public string UniqueNick { get; protected set; }
        public string PasswordHash { get; protected set; }
        public bool IsUniqueNickLogin { get; protected set; }

        public override bool Parse()
        {
            if (!base.Parse())
            {
                return false;
            }

            NameSpaceID = _cmdParams[0];
            if (_cmdParams.Contains("*"))
            {
                IsUniqueNickLogin = false;
                PasswordHash = _cmdParams[1];

                if (_longParam.Where(c => c.Equals("@")).Count() != 2)
                {
                    return false;
                }
                int profilenickIndex = _longParam.IndexOf("@");

                ProfileNick = _longParam.Substring(1, profilenickIndex - 1);
                Email = _longParam.Substring(profilenickIndex);
            }
            else
            {
                UniqueNick = _cmdParams[1];
                PasswordHash = _cmdParams[2];
            }

            return true;
        }
    }
}
