using Microsoft.AspNetCore.Mvc;
using System;
using System.Xml.Linq;
namespace RetroSpyServices.Sake
{
    public class StorageServer : Controller, IStorageServer
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
        public StorageServerModel TestStorageServerModel(StorageServerModel customModel)
        {
            return customModel;
        }

        public string upload()
        {
            throw new NotImplementedException();
        }
    }


}
