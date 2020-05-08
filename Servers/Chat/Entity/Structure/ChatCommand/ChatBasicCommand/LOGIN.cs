using System.Linq;

namespace Chat.Entity.Structure.ChatCommand
{
    public class LOGIN : ChatCommandBase
    {
        public string NameSpaceID { get; protected set; }
        public string ProfileNick { get; protected set; }
        public string Email { get; protected set; }
        public string UniqueNick { get; protected set; }
        public string PasswordHash { get; protected set; }
        public bool IsUniqueNickLogin { get; protected set; }

        public override bool Parse(string recv)
        {
            if (!base.Parse(recv))
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
