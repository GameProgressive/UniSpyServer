namespace UniSpyServer.Servers.PresenceConnectionManager.Test.Buddy
{
    public class BuddyRequests
    {
        public const string AddBuddy = @"\addbuddy\\sesskey\0\newprofileid\0\reason\test\final\";

        public const string DelBuddy = @"\delbuddy\\sesskey\0\delprofileid\0\final\";

        public const string InviteTo = @"\inviteto\\sesskey\0\productid\0\profileid\0\final\";

        //public const string StatusInfo = @"\inviteto\\sesskey\0\productid\0\profileid\0\final\";

        public const string Status = @"\status\0\statstring\test\locstring\test\final\";

    }
}
