using System.Collections.Generic;
using UniSpy.Server.Chat.Abstraction.BaseClass;
using UniSpy.Server.Chat.Error.IRC.General;

namespace UniSpy.Server.Chat.Contract.Request.General
{

    public sealed class NickRequest : RequestBase
    {
        private static List<char> _invalidChars = new List<char>() { '#', '@', '$', '%', '^', '&', '!', '~' };
        public NickRequest(string rawRequest) : base(rawRequest) { }

        public string NickName { get; private set; }

        public override void Parse()
        {
            base.Parse();

            if (_cmdParams?.Count == 1)
            {
                NickName = _cmdParams[0];
            }
            else if (_longParam is not null)
            {
                NickName = _longParam;
            }
            else
            {
                throw new Chat.Exception("NICK request is invalid.");
            }

            foreach (var c in _invalidChars)
            {
                if (NickName.Contains(c))
                {
                    var validNickName = NickName.Replace(c, '0');
                    throw new NickNameInUseException(
                    $"The nick name: {NickName} contains invalid character",
                    NickName,
                    validNickName);
                }
            }
        }
    }
}
