using UniSpy.Server.Chat.Abstraction.BaseClass;

namespace UniSpy.Server.Chat.Contract.Result.General
{
    public sealed class NickResult : ResultBase
    {
        public string NickName { get; set; }
        public NickResult()
        {
        }
    }
}