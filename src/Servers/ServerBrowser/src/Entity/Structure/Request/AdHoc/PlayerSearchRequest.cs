using UniSpyServer.ServerBrowser.Abstraction.BaseClass;
using System;
using UniSpyServer.UniSpyLib.Encryption;
using UniSpyServer.UniSpyLib.Extensions;

namespace UniSpyServer.ServerBrowser.Entity.Structure.Request
{
    public sealed class PlayerSearchRequest : RequestBase
    {
        public int SearchOption { get; private set; }
        public new int CommandName => SearchOption;
        public uint MaxResults { get; private set; }
        public string SearchName { get; private set; }
        public string Message { get; private set; }


        public PlayerSearchRequest(object rawRequest) : base(rawRequest)
        {
        }

        public override void Parse()
        {
            base.Parse();
            SearchOption = Convert.ToInt16(ByteTools.SubBytes(RawRequest, 3, 3 + 4));
            MaxResults = Convert.ToUInt16(ByteTools.SubBytes(RawRequest, 7, 7 + 4));

            int nameLength = BitConverter.ToInt32(
                ByteTools.SubBytes(RawRequest, 11, 11 + 4));
            SearchName = UniSpyEncoding.GetString(
                ByteTools.SubBytes(RawRequest, 15, nameLength));

            int messageLength = BitConverter.ToInt32(
                ByteTools.SubBytes(RawRequest, 15 + nameLength, 4));
            Message = UniSpyEncoding.GetString(
                ByteTools.SubBytes(RawRequest, 15 + nameLength + 4, messageLength));
        }
    }
}
