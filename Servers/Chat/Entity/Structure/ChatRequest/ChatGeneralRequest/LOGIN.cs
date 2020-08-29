using System.Linq;

namespace Chat.Entity.Structure.ChatCommand
{
    public enum LoginType
    {
        UniqueNickLogin,
        NickAndEmailLogin,
    }

    public class LOGIN : ChatRequestBase
    {
        public LOGIN(string rawRequest) : base(rawRequest)
        {
        }

        public LoginType RequestType { get; protected set; }
        public uint NameSpaceID { get; protected set; }
        public string NickName { get; protected set; }
        public string Email { get; protected set; }
        public string UniqueNick { get; protected set; }
        public string PasswordHash { get; protected set; }

        public override bool Parse()
        {
            if (!Parse())
            {
                return false;
            }

            uint namespaceid;

            if (!uint.TryParse(_cmdParams[0], out namespaceid))
            {
                return false;
            }

            NameSpaceID = namespaceid;

            if (_cmdParams[1] == "*")
            {
                RequestType = LoginType.NickAndEmailLogin;
                PasswordHash = _cmdParams[2];

                if (_longParam.Count(c => c == '@') != 2)
                {
                    return false;
                }
                int profilenickIndex = _longParam.IndexOf("@");

                NickName = _longParam.Substring(0, profilenickIndex);
                Email = _longParam.Substring(profilenickIndex + 1);
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
