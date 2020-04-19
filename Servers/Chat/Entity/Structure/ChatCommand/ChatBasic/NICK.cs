namespace Chat.Entity.Structure.ChatCommand
{
    public class NICK : ChatCommandBase
    {

        public string NickName { get; protected set; }

        public override string BuildRPL(params string[] cmdParam)
        {
            return BuildBasicRPL(ChatResponseType.Welcome, "Welcome to RetroSpy!", cmdParam[0]);
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
