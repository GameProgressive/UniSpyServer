using System;
namespace Chat.Entity.Structure.ChatCommand
{
    public class PING : ChatCommandBase
    {

        public string BuildResponse(params string[] param)
        {
            return BuildMessageRPL("","PING","www.rspy.cc");
        }
    }
}
