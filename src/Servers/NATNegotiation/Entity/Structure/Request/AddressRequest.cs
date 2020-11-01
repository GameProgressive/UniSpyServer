using System;
namespace NATNegotiation.Entity.Structure.Request
{
    public class AddressRequest : InitRequest
    {
        public AddressRequest(byte[] rawRequest) : base(rawRequest)
        {
        }
    }
}
