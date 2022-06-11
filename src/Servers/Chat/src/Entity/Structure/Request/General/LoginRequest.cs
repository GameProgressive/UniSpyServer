using UniSpyServer.Servers.Chat.Abstraction.BaseClass;

using System.Linq;

namespace UniSpyServer.Servers.Chat.Entity.Structure.Request.General
{
    public enum LoginReqeustType
    {
        UniqueNickLogin,
        NickAndEmailLogin,
    }
    
    public sealed class LoginRequest : RequestBase
    {
        public LoginRequest(string rawRequest) : base(rawRequest){ }

        public LoginReqeustType ReqeustType { get; private set; }
        public int NamespaceID { get; private set; }
        public string NickName { get; private set; }
        public string Email { get; private set; }
        public string UniqueNick { get; private set; }
        public string PasswordHash { get; private set; }

        public override void Parse()
        {
            base.Parse();

            int namespaceid;
            if (!int.TryParse(_cmdParams[0], out namespaceid))
            {
                throw new Exception.ChatException("The namespaceid format is incorrect.");
            }
            NamespaceID = namespaceid;

            if (_cmdParams[1] == "*")
            {
                ReqeustType = LoginReqeustType.NickAndEmailLogin;
                PasswordHash = _cmdParams[2];

                if (_longParam.Count(c => c == '@') != 2)
                {
                    throw new Exception.ChatException("The profile nick format is incorrect.");
                }

                int profilenickIndex = _longParam.IndexOf("@");
                NickName = _longParam.Substring(0, profilenickIndex);
                Email = _longParam.Substring(profilenickIndex + 1);
                return;
            }

            ReqeustType = LoginReqeustType.UniqueNickLogin;
            UniqueNick = _cmdParams[1];
            PasswordHash = _cmdParams[2];
        }
    }
}
