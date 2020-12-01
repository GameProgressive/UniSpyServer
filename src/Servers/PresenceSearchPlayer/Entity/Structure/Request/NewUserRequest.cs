using UniSpyLib.MiscMethod;
using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;
using System.Collections.Generic;

namespace PresenceSearchPlayer.Entity.Structure.Request
{
    public class NewUserRequest : PSPRequestBase
    {
        public uint ProductID { get; protected set; }
        public uint GamePort { get; protected set; }
        public string CDKeyEnc { get; protected set; }
        public bool HasGameNameFlag { get; protected set; }
        public bool HasProductIDFlag { get; protected set; }
        public bool HasCDKeyEncFlag { get; protected set; }
        public bool HasPartnerIDFlag { get; protected set; }
        public bool HasGamePortFlag { get; protected set; }

        public string Nick { get; private set; }
        public string Email { get; private set; }
        public string Password { get; private set; }
        public string Uniquenick { get; private set; }
        public uint PartnerID { get; private set; }
        public string GameName { get; private set; }
        public uint NamespaceID { get; protected set; }
        public NewUserRequest(Dictionary<string, string> recv) : base(recv)
        {
        }

        public override object Parse()
        {
            var flag = (GPErrorCode)base.Parse();
            if (flag != GPErrorCode.NoError)
            {
                return flag;
            }
            string md5Password;
            if (!PasswordEncoder.ProcessPassword(_recv, out md5Password))
            {
                return GPErrorCode.NewUserBadPasswords;
            }
            Password = md5Password;

            if (!_recv.ContainsKey("nick"))
            {
                return GPErrorCode.Parse;
            }

            if (!_recv.ContainsKey("email") || !GameSpyUtils.IsEmailFormatCorrect(_recv["email"]))
            {
                return GPErrorCode.Parse;
            }

            Nick = _recv["nick"];
            Email = _recv["email"];

            if (_recv.ContainsKey("uniquenick") && _recv.ContainsKey("namespaceid"))
            {
                if (_recv.ContainsKey("namespaceid"))
                {
                    uint namespaceID;
                    if (!uint.TryParse(_recv["namespaceid"], out namespaceID))
                    {
                        return GPErrorCode.Parse;
                    }

                    NamespaceID = namespaceID;
                }
                Uniquenick = _recv["uniquenick"];
            }
            return ParseOtherInfo();
        }

        private GPErrorCode ParseOtherInfo()
        {

            //parse other info
            if (_recv.ContainsKey("partnerid"))
            {
                uint partnerid;
                if (!uint.TryParse(_recv["partnerid"], out partnerid))
                {
                    return GPErrorCode.Parse;
                }
                HasPartnerIDFlag = true;
                PartnerID = partnerid;
            }



            if (_recv.ContainsKey("productid"))
            {
                uint productid;
                if (!uint.TryParse(_recv["productid"], out productid))
                {
                    return GPErrorCode.Parse;
                }
                HasProductIDFlag = true;
                ProductID = productid;
            }

            if (_recv.ContainsKey("gamename"))
            {
                HasGameNameFlag = true;
                GameName = _recv["gamename"];
            }


            if (_recv.ContainsKey("port"))
            {
                uint port;
                if (!uint.TryParse(_recv["port"], out port))
                {
                    return GPErrorCode.Parse;
                }
                HasGamePortFlag = true;
                GamePort = port;
            }

            if (_recv.ContainsKey("cdkeyenc"))
            {
                HasCDKeyEncFlag = true;
                CDKeyEnc = _recv["cdkeyenc"];
            }

            return GPErrorCode.NoError;
        }
    }
}
