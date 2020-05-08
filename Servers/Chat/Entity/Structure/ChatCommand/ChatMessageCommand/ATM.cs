namespace Chat.Entity.Structure.ChatCommand
{
    public enum ATMCmdType
    {
        UserATM,
        ChannelATM
    }

    public class ATM : ChatMessageCommandBase
    {
        public ATMCmdType RequestType { get; protected set; }
        public string NickName { get; protected set; }

        public override bool Parse(string recv)
        {
            if (!base.Parse(recv))
            {
                return false;
            }
            if (_cmdParams.Count != 2)
            {
                return false;
            }

            if (_cmdParams[0].Contains("#"))
            {
                RequestType = ATMCmdType.ChannelATM;
            }
            else
            {
                RequestType = ATMCmdType.UserATM;
                NickName = _cmdParams[0];
            }

            return true;
        }
    }

}
