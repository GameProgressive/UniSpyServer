using System;
using System.Linq;
using UniSpy.Server.ServerBrowser.V2.Abstraction.BaseClass;
using UniSpy.Server.Core.Encryption;

namespace UniSpy.Server.ServerBrowser.V2.Contract.Request
{

    public sealed class PlayerSearchRequest : RequestBase
    {
        public int SearchOption { get; private set; }
        public new int CommandName => SearchOption;
        public int MaxResults { get; private set; }
        public string SearchName { get; private set; }
        public string Message { get; private set; }


        public PlayerSearchRequest(byte[] rawRequest) : base(rawRequest)
        {
        }

        public override void Parse()
        {
            base.Parse();
            SearchOption = Convert.ToInt16(RawRequest.Skip(3).Take(4).ToArray());
            MaxResults = Convert.ToInt16(RawRequest.Skip(7).Take(4).ToArray());

            int nameLength = BitConverter.ToInt32(RawRequest.Skip(11).Take(4).ToArray());
            SearchName = UniSpyEncoding.GetString(RawRequest.Skip(15).Take(nameLength).ToArray());

            int messageLength = BitConverter.ToInt32(RawRequest.Skip(15).Take(4).ToArray());
            Message = UniSpyEncoding.GetString(RawRequest.Skip(15 + nameLength + 4).Take(messageLength).ToArray());
        }
    }
}
