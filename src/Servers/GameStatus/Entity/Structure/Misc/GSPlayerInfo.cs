namespace GameStatus.Entity.Structure.Misc
{
    internal class GSPlayerInfo
    {
        public uint SessionKey;
        public static string Challenge = "00000000000000000000";
        public static string ChallengeResponse = $@"\challenge\{Challenge}\final\";
        public string GameName;
    }
}
