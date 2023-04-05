using System.Collections.Generic;
using System.Linq;
using UniSpy.Server.Core.Misc;

namespace UniSpy.Server.ServerBrowser.V1.Abstraction.BaseClass
{
    public abstract class RequestBase : UniSpy.Server.Core.Abstraction.BaseClass.RequestBase
    {
        public new string RawRequest { get => (string)base.RawRequest; protected set => base.RawRequest = value; }
        public new string CommandName { get => (string)base.CommandName; protected set => base.CommandName = value; }
        public Dictionary<string, string> KeyValues { get; private set; }
        public string GameName { get; private set; }
        public bool IsUsingEncryption { get; set; }
        public RequestBase(string rawRequest) : base(rawRequest)
        {
        }
        public override void Parse()
        {
            KeyValues = GameSpyUtils.ConvertToKeyValue(RawRequest);
            CommandName = KeyValues.Keys.First();
            if (KeyValues.ContainsKey("secure"))
            {
                IsUsingEncryption = true;
            }
            if (!KeyValues.ContainsKey("gamename"))
            {
                throw new ServerBrowser.V2.Exception("No game name present in request.");
            }
            GameName = KeyValues["gamename"];
        }
    }
}