using PresenceConnectionManager.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;
using System.Collections.Generic;

namespace PresenceConnectionManager.Entity.Structure.Request.Profile
{
    public class NewProfileRequest : PCMRequestBase
    {
        //create a new profile with new nick 
        // @"  \newprofile\sesskey\<>\nick\<>\id\1\final\"
        //replace a existed nick with new nick
        //@"  \newprofile\sesskey\<>\nick\<>\replace\1\oldnick\<>\id\1\final\"

        public NewProfileRequest(string rawRequest) : base(rawRequest)
        {
        }

        public string OldNick { get; protected set; }
        public string NewNick { get; protected set; }
        public bool IsReplaceNickName { get; protected set; }
        public override object Parse()
        {
            var flag = (GPErrorCode)base.Parse();
            if (flag != GPErrorCode.NoError)
            {
                return flag;
            }

            if (KeyValues.ContainsKey("replace"))
            {
                if (!KeyValues.ContainsKey("oldnick") && !KeyValues.ContainsKey("nick"))
                {
                    return GPErrorCode.Parse;
                }

                OldNick = KeyValues["oldnick"];
                NewNick = KeyValues["nick"];
                IsReplaceNickName = true;
            }
            else
            {
                if (!KeyValues.ContainsKey("nick"))
                {
                    return GPErrorCode.Parse;
                }
                NewNick = KeyValues["nick"];
                IsReplaceNickName = false;
            }

            return GPErrorCode.NoError;
        }
    }
}
