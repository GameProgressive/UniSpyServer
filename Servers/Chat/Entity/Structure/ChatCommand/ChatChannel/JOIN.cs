namespace Chat.Entity.Structure.ChatCommand
{
    public class JOIN : ChatChannelCommandBase
    {

        public string Password { get; protected set; }
        public override bool Parse(string request)
        {
            bool flag = base.Parse(request);
            if (_cmdParams.Count != 2)
            {
                return false;
            }
            Password = _cmdParams[1];
            return flag;
        }
    }
}
