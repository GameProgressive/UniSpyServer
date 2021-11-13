namespace UniSpyServer.Servers.PresenceSearchPlayer.Test
{
    public class RawRequests
    {
        /// <summary>
        /// Optional parameter: nick, uniquenick, email, firstname, lastname, icquin, skip
        /// </summary>
        public const string Profile = @"\search\\sesskey\123456\profileid\123\namespaceid\0\nick\xiaojiuwo\uniquenick\xiaojiuwo\email\xiaojiuwo@gamespy.com\firstname\xiao\lastname\jiuwo\icquin\123\skip\0\gamename\gmtest\final\";

        public const string ProfileUniquenick = @"\searchunique\\sesskey\123456\profileid\0\uniquenick\xiaojiuwo\namespaces\1,2,3,4,5\gamename\gmtest\final\";
        public const string IsValid = @"\valid\\email\xiaojiuwo@gamespy.com\partnerid\123\gamename\gmtest\final\";
        public const string Nick = @"\nicks\\email\xiaojiuwo@gamespy.com\passenc\xxxxx\namespaceid\0\partnerid\0\gamename\gmtest\final\";
        public const string Players = @"\pmatch\\sesskey\123456\profileid\0\productid\0\final\";
        public const string Check = @"\check\\nick\xiaojiuwo\email\xiaojiuwo@gamespy.com\partnerid\0\passenc\xxxx\gamename\gmtest\final\";
        /// <summary>
        /// Optional parameter: cdkey
        /// </summary>
        public const string NewUser = @"\newuser\\nick\xiaojiuwo\email\xiaojiuwo@gamespy.com\passenc\xxxx\productID\0\namespaceid\0\uniquenick\xiaojiuwo\cdkey\xxx-xxx-xxx-xxx\partnerid\0\gamename\gmtest\final\";
        public const string OthersBuddy = @"\others\\sesskey\123456\profileid\0\namespaceid\0\gamename\gmtest\final\";
        public const string OthersBuddyList = @"\otherlist\\sesskey\123456\profileid\0\numopids\2\opids\1|2\namespaceid\0\gamename\gmtest\final\";
        public const string SuggestUnique = @"uniquesearch\\preferrednick\xiaojiuwo\namespaceid\0\gamename\gmtest\final\";

    }
}