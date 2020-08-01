using System;
using System.Collections.Generic;
using PresenceConnectionManager.Entity.BaseClass;
using PresenceSearchPlayer.Entity.Enumerator;

namespace PresenceConnectionManager.Entity.Structure.Request.Profile
{
    public class NewProfileRequest : PCMRequestBase
    {
        //create a new profile with new nick 
        // @"  \newprofile\sesskey\<>\nick\<>\id\1\final\"
        //replace a existed nick with new nick
        //@"  \newprofile\sesskey\<>\nick\<>\replace\1\oldnick\<>\id\1\final\"

        public NewProfileRequest(Dictionary<string, string> recv) : base(recv)
        {
        }

        public string OldNick { get; protected set; }
        public string NewNick { get; protected set; }
        public bool IsReplaceNickName { get; protected set; }
        public override GPError  Parse()
        {
            var flag = base.Parse();
            if (flag != GPError.NoError)
            {
                return flag;
            }

            if (_recv.ContainsKey("replace"))
            {
                if (!_recv.ContainsKey("oldnick") && !_recv.ContainsKey("nick"))
                {
                    return GPError.Parse;
                }

                OldNick = _recv["oldnick"];
                NewNick = _recv["nick"];
                IsReplaceNickName = true;
            }
            else
            {
                if (!_recv.ContainsKey("nick"))
                {
                    return GPError.Parse;
                }
                NewNick = _recv["nick"];
                IsReplaceNickName = false;
            }

            return GPError.NoError;
        }
    }
}
