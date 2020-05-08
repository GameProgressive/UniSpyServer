namespace Chat.Entity.Structure.ChatCommand
{
    public class NICK : ChatCommandBase
    {

        public string NickName { get; protected set; }

        public override bool Parse(string recv)
        {
            if (!base.Parse(recv))
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
