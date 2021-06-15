using Chat.Abstraction.BaseClass;
using Chat.Entity.Exception;
using System.Linq;

namespace Chat.Entity.Structure.Request.General
{
    public enum LoginType
    {
        UniqueNickLogin,
        NickAndEmailLogin,
    }

    internal sealed class LOGINRequest : ChatRequestBase
    {
        public LOGINRequest(string rawRequest) : base(rawRequest)
        {
        }

        public LoginType RequestType { get; private set; }
        public uint NameSpaceID { get; private set; }
        public string NickName { get; private set; }
        public string Email { get; private set; }
        public string UniqueNick { get; private set; }
        public string PasswordHash { get; private set; }

        public override void Parse()
        {
            base.Parse();



            uint namespaceid;

            if (!uint.TryParse(_cmdParams[0], out namespaceid))
            {
                throw new ChatException("namespaceid format is incorrect.");
            }

            NameSpaceID = namespaceid;

            if (_cmdParams[1] == "*")
            {
                RequestType = LoginType.NickAndEmailLogin;
                PasswordHash = _cmdParams[2];

                if (_longParam.Count(c => c == '@') != 2)
                {
                    throw new ChatException("The profile nick format is incorrect.");
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
        }
    }
}
