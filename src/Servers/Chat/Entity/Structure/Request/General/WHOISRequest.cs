using Chat.Abstraction.BaseClass;
using Chat.Entity.Exception;

namespace Chat.Entity.Structure.Request.General
{
    public class WHOISRequest : ChatRequestBase
    {
        public WHOISRequest(string rawRequest) : base(rawRequest)
        {
        }

        public string NickName { get; protected set; }

        public override void Parse()
        {
            base.Parse();


            if (_cmdParams.Count != 1)
            {
                throw new ChatException("The number of IRC cmd params in GETKEY request is incorrect.");
            }

            NickName = _cmdParams[0];
        }
    }
}
