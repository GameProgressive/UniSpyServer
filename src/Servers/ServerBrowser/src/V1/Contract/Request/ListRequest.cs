using UniSpy.Server.ServerBrowser.V1.Abstraction.BaseClass;

namespace UniSpy.Server.ServerBrowser.V1.Contract.Request
{
    public enum ListRequestType
    {
        Basic,
        Info,
        Group
    }
    /// <summary>
    /// Query for game server list of specific game name with filter
    /// </summary>
    public sealed class ListRequest : RequestBase
    {
        public string Filter { get; private set; }
        public bool IsSendingCompressFormat { get; private set; }
        public bool IsSendAllInfo { get; private set; }
        public ListRequestType? Type { get; private set; }
        public ListRequest(string rawRequest) : base(rawRequest)
        {
        }
        public override void Parse()
        {
            // base.Parse();
            var firstGameIndex = RawRequest.IndexOf('\\', 10);
            var gameNameHeader = RawRequest.Substring(0, firstGameIndex);


            if (KeyValues.ContainsKey("where"))
            {
                Filter = KeyValues["where"];
            }
            switch (KeyValues[CommandName])
            {
                case "cmp":
                    IsSendingCompressFormat = true;
                    Type = ListRequestType.Basic;
                    break;
                case "info2":
                    IsSendAllInfo = true;
                    Type = ListRequestType.Info;
                    break;
                case "groups":
                    Type = ListRequestType.Group;
                    IsSendingCompressFormat = true;
                    IsSendAllInfo = true;
                    break;
            }
        }
    }
}