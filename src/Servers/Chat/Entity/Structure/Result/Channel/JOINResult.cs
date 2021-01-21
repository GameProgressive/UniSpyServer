using Chat.Abstraction.BaseClass;

namespace Chat.Entity.Structure.Result
{
    internal sealed class JOINResult : ChatResultBase
    {
        public string JoinerPrefix { get; set; }
        public JOINResult()
        {
        }
    }
}
