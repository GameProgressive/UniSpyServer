namespace Chat.Entity.Structure.ChatCommand
{
    public class NICK : ChatCommandBase
    {

        public string NickName { get; protected set; }

        public string BuildResponse(string nickName)
        {
            return BuildRPL("www.rspy.cc",ChatResponseType.Welcome, nickName,"Welcome to RetroSpy!");
        }

        public override bool Parse(string request)
        {
            if (!base.Parse(request))
            {
                return false;
            }
            NickName = _cmdParams[0];
            return true;
        }
    }
}
