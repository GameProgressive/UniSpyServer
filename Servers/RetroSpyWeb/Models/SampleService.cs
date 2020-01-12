using Models;
using System;
using System.Xml.Linq;
namespace Models
{
    public class SampleService : ISampleService
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
        public MyCustomModel TestCustomModel(MyCustomModel customModel)
        {
            return customModel;
        }
    }


}
