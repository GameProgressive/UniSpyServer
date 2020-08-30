using System;
using System.Xml.Linq;
using RetroSpyServices.PatchingAndTracking.Entity.Interface;
using RetroSpyServices.PatchingAndTracking.Entity.Structure.Model;

namespace RetroSpyServices.PatchingAndTracking.Service
{
    public class PatchingAndTrackingService : IPatchingAndTrackingService
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
        public PatchingAndTrackingServiceModel TestPatchingAndTrackingServiceModel(PatchingAndTrackingServiceModel customModel)
        {
            return customModel;
        }
    }


}
