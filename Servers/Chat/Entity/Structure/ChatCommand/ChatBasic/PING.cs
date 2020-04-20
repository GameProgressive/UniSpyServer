using System;
namespace Chat.Entity.Structure.ChatCommand
{
    public class PING : ChatCommandBase
    {

        public string BuildResponse(params string[] param)
        {
            return BuildMessage("","PING","www.rspy.cc");
        }
    }
}
