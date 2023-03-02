using System.Collections.Generic;
using System.Linq;
using UniSpy.Server.Core.MiscMethod;

namespace UniSpy.Server.Master.Abstraction.BaseClass
{
    public abstract class RequestBase : UniSpy.Server.Core.Abstraction.BaseClass.RequestBase
    {
        public new string CommandName { get => (string)base.CommandName; protected set => base.CommandName = value; }
        public new string RawRequest => (string)base.RawRequest;
        public Dictionary<string, string> RequestKeyValues { get; protected set; }
        public RequestBase(string rawRequest) : base(rawRequest)
        {
        }
        public override void Parse()
        {
            RequestKeyValues = GameSpyUtils.ConvertToKeyValue(RawRequest);
            CommandName = RequestKeyValues.Keys.First();
        }
    }
}