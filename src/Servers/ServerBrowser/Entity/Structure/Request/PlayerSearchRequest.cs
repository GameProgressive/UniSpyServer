using System;
using System.Text;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Extensions;

namespace ServerBrowser.Entity.Structure.Request
{
    internal sealed class PlayerSearchRequest : UniSpyRequestBase
    {
        public int SearchOption { get; private set; }
        public new int CommandName => SearchOption;
        public new byte[] RawRequest => (byte[])base.RawRequest;
        public uint MaxResults { get; private set; }
        public string SearchName { get; private set; }
        public string Message { get; private set; }


        public PlayerSearchRequest(object rawRequest) : base(rawRequest)
        {
        }

        public override void Parse()
        {
            SearchOption = Convert.ToInt16(ByteTools.SubBytes(RawRequest, 3, 3 + 4));
            MaxResults = Convert.ToUInt16(ByteTools.SubBytes(RawRequest, 7, 7 + 4));

            int nameLength = BitConverter.ToInt32(
                ByteTools.SubBytes(RawRequest, 11, 11 + 4));
            SearchName = Encoding.ASCII.GetString(
                ByteTools.SubBytes(RawRequest, 15, nameLength));

            int messageLength = BitConverter.ToInt32(
                ByteTools.SubBytes(RawRequest, 15 + nameLength, 4));
            Message = Encoding.ASCII.GetString(
                ByteTools.SubBytes(RawRequest, 15 + nameLength + 4, messageLength));

            ErrorCode = true;
        }
    }
}
