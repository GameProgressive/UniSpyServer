using UniSpy.Server.Chat.Abstraction.BaseClass;


namespace UniSpy.Server.Chat.Contract.Request.General
{
    
    public sealed class UserIPRequest : RequestBase
    {
        public UserIPRequest(string rawRequest) : base(rawRequest){ }

        public override void Parse()
        {
            base.Parse();
            // USRIP content is empty!
        }
    }
}
