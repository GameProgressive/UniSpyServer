using System;
using System.Xml.Linq;
using RetroSpyServices.Competitive.Entity.Interface;
using RetroSpyServices.Competitive.Entity.Structure.Model;

namespace RetroSpyServices.Competitive.Service
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
