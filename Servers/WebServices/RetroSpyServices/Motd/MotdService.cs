using System;
using System.Xml.Linq;
namespace RetroSpyServices.Motd
{
    public class MotdService : IMotdService
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
        public MotdServiceModel TestMotdServiceModel(MotdServiceModel customModel)
        {
            return customModel;
        }
    }


}
