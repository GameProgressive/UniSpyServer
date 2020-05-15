using System.Linq;

namespace Chat.Entity.Structure.ChatCommand
{
    public enum LoginType
    {
        UniqueNickLogin,
        NickAndEmailLogin,
    }

    public class LOGIN : ChatCommandBase
    {
        public LoginType RequestType { get; protected set; }
        public uint NameSpaceID { get; protected set; }
        public string NickName { get; protected set; }
        public string Email { get; protected set; }
        public string UniqueNick { get; protected set; }
        public string PasswordHash { get; protected set; }

        public override bool Parse(string recv)
        {
            if (!base.Parse(recv))
            {
                return false;
            }

            uint namespaceid;

            if (!uint.TryParse(_cmdParams[0], out namespaceid))
            {
                return false;
            }

            NameSpaceID = namespaceid;

            if (_cmdParams.Contains("*"))
            {
                RequestType = LoginType.NickAndEmailLogin;
                PasswordHash = _cmdParams[1];

                if (_longParam.Where(c => c.Equals("@")).Count() != 2)
                {
                    return false;
                }
                int profilenickIndex = _longParam.IndexOf("@");

                NickName = _longParam.Substring(1, profilenickIndex - 1);
                Email = _longParam.Substring(profilenickIndex);
            }
            else
            {
                RequestType = LoginType.UniqueNickLogin;
                UniqueNick = _cmdParams[1];
                PasswordHash = _cmdParams[2];
            }

            return true;
        }
    }
}
