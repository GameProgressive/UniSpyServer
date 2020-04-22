using System;
using System.Collections.Generic;
using System.Linq;
namespace Chat.Entity.Structure.ChatCommand
{
    public class MODE : ChatCommandBase
    {
        //request :
        //"MODE %s +q", connection->nick);
        //"MODE %s -q", connection->nick);

        //"MODE %s +k %s", channel, password);
        //"MODE %s -k %s", channel, password);

        // "MODE %s +l %d", channel, limit);
        //"MODE %s -l", channel);

        //"MODE %s +b", channel);
        //"MODE %s +b %s", channel, ban
        //"MODE %s -b %s", channel, ban);

        //"MODE %s %co %s", channel, sign, user);
        //"MODE %s %cv %s", channel, sign, user);

        public string ChannelName { get; protected set; }
        public List<string> Modes { get; protected set; }

        public override bool Parse(string request)
        {
            if (!base.Parse(request))
            {
                return false;
            }

            if (_cmdParams.Count == 1)
            {
                ChannelName = _cmdParams[0];
                return true;
            }

            foreach (var c in _cmdParams.Skip(1))
            {
                Modes.Add(c);
            }
            return true;
        }

        public string GenerateResponse(string modes)
        {
            return BuildMessageRPL("www.rspy.cc", $"MODE {ChannelName} {modes}", "");
        }
    }
}
