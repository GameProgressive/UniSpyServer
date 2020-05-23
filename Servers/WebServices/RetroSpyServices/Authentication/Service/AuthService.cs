using System;
using System.Xml.Linq;
using RetroSpyServices.Authentication.Entity.Interface;
using RetroSpyServices.Authentication.Entity.Structure.Model;

namespace RetroSpyServices.Authentication.Service
{
    public class AuthService : IAuthService
    {
        public string LoginUniqueNick(string s)
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
