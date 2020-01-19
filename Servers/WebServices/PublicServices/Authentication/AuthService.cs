using System;
using System.Xml.Linq;
namespace PublicServices.Authentication
{
    public class AuthService : IAuthService
    {
        public string Test(string s)
        {
            Console.WriteLine("Test Method Executed!");
            return s;
        }
        public void XmlMethod(XElement xml)
        {
            Console.WriteLine(xml.ToString());
        }
        public AuthServiceModel TestAuthServiceModel(AuthServiceModel customModel)
        {
            return customModel;
        }
    }


}
