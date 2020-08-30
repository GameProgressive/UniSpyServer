using System.Collections.Generic;
using GameSpyLib.MiscMethod;
using PresenceSearchPlayer.Entity.Enumerator;
using PresenceSearchPlayer.Entity.Structure.Model;

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

        public override GPError Parse()
        {
            var flag = base.Parse();
            if (flag != GPError.NoError)
            {
                return flag;
            }
            string md5Password;
            if (!PasswordEncoder.ProcessPassword(_rawRequest, out md5Password))
            {
                return GPError.NewUserBadPasswords;
            }
            Password = md5Password;

            if (!_rawRequest.ContainsKey("nick"))
            {
                return GPError.Parse;
            }

            if (!_rawRequest.ContainsKey("email") || !GameSpyUtils.IsEmailFormatCorrect(_rawRequest["email"]))
            {
                return GPError.Parse;
            }


            Nick = _rawRequest["nick"];
            Email = _rawRequest["email"];

            if (_rawRequest.ContainsKey("uniquenick")&&_rawRequest.ContainsKey("namespaceid"))
            {
                if (_rawRequest.ContainsKey("namespaceid"))
                {
                    uint namespaceID;
                    if (!uint.TryParse(_rawRequest["namespaceid"], out namespaceID))
                    {
                        return GPError.Parse;
                    }

                    NamespaceID = namespaceID;
                }
                Uniquenick = _rawRequest["uniquenick"];
            }
            return ParseOtherInfo();
        }

        private GPError ParseOtherInfo()
        {

            //parse other info
            if (_rawRequest.ContainsKey("partnerid"))
            {
                uint partnerid;
                if (!uint.TryParse(_rawRequest["partnerid"], out partnerid))
                {
                    return GPError.Parse;
                }
                HasPartnerIDFlag = true;
                PartnerID = partnerid;
            }



            if (_rawRequest.ContainsKey("productid"))
            {
                uint productid;
                if (!uint.TryParse(_rawRequest["productid"], out productid))
                {
                    return GPError.Parse;
                }
                HasProductIDFlag = true;
                ProductID = productid;
            }

            if (_rawRequest.ContainsKey("gamename"))
            {
                HasGameNameFlag = true; 
                GameName = _rawRequest["gamename"];
            }


            if (_rawRequest.ContainsKey("port"))
            {
                uint port;
                if (!uint.TryParse(_rawRequest["port"], out port))
                {
                    return GPError.Parse;
                }
                HasGamePortFlag = true;
                GamePort = port;
            }

            if (_rawRequest.ContainsKey("cdkeyenc"))
            {
                HasCDKeyEncFlag = true;
                CDKeyEnc = _rawRequest["cdkeyenc"];
            }

            return GPError.NoError;
        }
    }
}
