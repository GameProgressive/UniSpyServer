namespace Chat.Entity.Structure.ChatCommand
{
    public class USRIP : ChatCommandBase
    {
        public string BuildResponse(string ip)
        {
            return BuildNumericRPL("", ChatResponseType.UserIP, "", $"@{ip}");
        }
    }
}
