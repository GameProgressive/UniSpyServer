using System;
using System.Xml.Linq;
using RetroSpyServices.Sake.Entity.Interface;
using RetroSpyServices.Sake.Entity.Structure.Model;

namespace RetroSpyServices.Sake.Service
{
    public class SakeStorageService : ISakeStorageService
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

        public SakeStorageModel TestStorageServerModel(SakeStorageModel customModel)
        {
            return customModel;
        }

        public string Upload()
        {
            throw new NotImplementedException();
        }
    }


}
