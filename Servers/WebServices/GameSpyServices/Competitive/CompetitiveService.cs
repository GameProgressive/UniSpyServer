using System;
using System.Xml.Linq;
namespace PublicServices.Competitive
{
    public class CompetitiveService : ICompetitiveService
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
        public CompetitiveServiceModel TestCompetitiveServiceModel(CompetitiveServiceModel customModel)
        {
            return customModel;
        }
    }


}
