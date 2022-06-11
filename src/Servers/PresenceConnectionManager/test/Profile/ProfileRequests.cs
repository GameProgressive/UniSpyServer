namespace UniSpyServer.Servers.PresenceConnectionManager.Test
{
    public static class ProfileRequests
    {
        public const string AddBlock = @"\addblock\\profileid\0\final\";

        public const string GetProfile = @"\getprofile\\sesskey\xxxx\profileid\0\final\";

        public const string NewProfile = @"\newprofile\\sesskey\xxxx\nick\spyguy\id\1\final\";

        public const string NewProfileReplace = @"\newprofile\\sesskey\xxxx\nick\spyguy2\replace\1\oldnick\spyguy\id\1\final\";

        public const string RegisterCDKey = @"\registercdkey\\sesskey\xxxx\cdkeyenc\xxxx\id\1\final\";

        public const string RegisterNick = @"\registernick\\sesskey\xxxx\uniquenick\spyguy\partnerid\0\id\1\final\";

        //public const string UpdatePro = @"\updatepro\\sesskey\xxxx\firstname\Spy\lastname\Guy\icquin\0\homepage\unispy.org\zipcode\00000\countrycode\US\birthday\335742908\sex\0\aim\spyguy@aim.com\pic\0\occ\0\ind\0\inc\0\mar\0\chc\0\i1\0\nick\spyguy\uniquenick\spyguy\publicmask\0\partnerid\0\final\";

        //public const string UpdateUi = @"\updateui\\sesskey\xxxx\passwordenc\4a7d1ed414474e4033ac29ccb8653d9b\email\spyguy@unispy.org\final\";
    }
}
