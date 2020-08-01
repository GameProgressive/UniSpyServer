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
        public string PassEnc { get; private set; }
        public string Uniquenick { get; private set; }
        public uint PartnerID { get; private set; }
        public string GameName { get; private set; }

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

            if (!_recv.ContainsKey("nick"))
            {
                return GPError.Parse;
            }

            if (!_recv.ContainsKey("email") || !GameSpyUtils.IsEmailFormatCorrect(_recv["email"]))
            {
                return GPError.Parse;
            }

            if (!_recv.ContainsKey("passenc"))
            {
                return GPError.Parse;
            }
            Nick = _recv["nick"];
            Email = _recv["email"];
            PassEnc = _recv["passenc"];

            if (_recv.ContainsKey("uniquenick"))
            {
                Uniquenick = _recv["uniquenick"];
            }

            return ParseOtherInfo();
        }

        private GPError ParseOtherInfo()
        {

            //parse other info
            if (_recv.ContainsKey("partnerid"))
            {
                uint partnerid;
                if (!uint.TryParse(_recv["partnerid"], out partnerid))
                {
                    return GPError.Parse;
                }
                PartnerID = partnerid;
            }



            if (_recv.ContainsKey("productid"))
            {
                uint productid;
                if (uint.TryParse(_recv["productid"], out productid))
                {
                    return GPError.Parse;
                }
                ProductID = productid;
            }

            if (_recv.ContainsKey("gamename"))
            {
                GameName = _recv["gamename"];
            }


            if (_recv.ContainsKey("port"))
            {
                uint port;
                if (!uint.TryParse(_recv["port"], out port))
                {
                    return GPError.Parse;
                }
                GamePort = port;
            }

            if (_recv.ContainsKey("cdkeyenc"))
            {
                CDKeyEnc = _recv["cdkeyenc"];
            }

            return GPError.NoError;
        }
    }
}
