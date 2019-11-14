using System;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Handler.SoapAuthHandler
{
    public class SoapAuthenticationService : SoapAuthServerBase
    {
        public string Ping(string s)
        {
            Console.WriteLine("Exec ping method");
            return s;
        }

        public AuthenticationResponse PingComplexModel(AuthenticationRequest clientRequest)
        {
            Console.WriteLine("Input data. IntProperty: {0}, StringProperty: {1}", clientRequest.IntProperty, clientRequest.StringProperty);

            return new AuthenticationResponse
            {
                FloatProperty = float.MaxValue / 2,
                StringProperty = clientRequest.StringProperty,
                ListProperty = clientRequest.ListProperty,
                DateTimeOffsetProperty = clientRequest.DateTimeOffsetProperty
            };
        }

        public void VoidMethod(out string s)
        {
            s = "Value from server";
        }

        public Task<int> AsyncMethod()
        {
            return Task.Run(() => 42);
        }

        public int? NullableMethod(bool? arg)
        {
            return null;
        }

        public void XmlMethod(XElement xml)
        {
            Console.WriteLine(xml.ToString());
        }
    }
}