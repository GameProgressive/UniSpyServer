using System;
using System.Xml.Linq;
namespace Sake
{
    public class StorageServer : IStorageServer
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
    }


}
