namespace Chat.Entity.Structure.ChatCommand
{
    public class NICK : ChatCommandBase
    {

        public string NickName { get; protected set; }

        public override bool Parse(string request)
        {
            if (!base.Parse(request))
            {
                return false;
            }
            if (_cmdParams.Count == 0)
            {
                NickName = _longParam;
            }
            else
            {
                NickName = _cmdParams[0];
            }
            return true;
        }
    }
}
