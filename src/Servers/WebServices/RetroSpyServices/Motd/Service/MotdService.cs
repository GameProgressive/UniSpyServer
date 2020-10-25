using RetroSpyServices.Motd.Entity.Interface;
using RetroSpyServices.Motd.Entity.Structure.Model;
using System;
using System.Xml.Linq;

namespace RetroSpyServices.Motd.Service
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
