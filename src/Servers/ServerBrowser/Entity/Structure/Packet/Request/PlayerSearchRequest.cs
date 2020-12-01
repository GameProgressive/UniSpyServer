using UniSpyLib.Extensions;
using System;
using System.Text;
using UniSpyLib.Abstraction.Interface;

namespace ServerBrowser.Entity.Structure.Packet.Request
{
    public class PlayerSearchRequest : IRequest
    {
        public int SearchOption { get; protected set; }
        public uint MaxResults { get; protected set; }

        public string SearchName { get; protected set; }

        public string Message { get; protected set; }

        public byte[] RawRequest { get; protected set; }

        object IRequest.CommandName => SearchOption;

        public PlayerSearchRequest(byte[] rawRequest)
        {
            RawRequest = rawRequest;
        }

        public override object Parse()
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

            return true;
        }

        object IRequest.Parse()
        {
            return Parse();
        }
    }
}
