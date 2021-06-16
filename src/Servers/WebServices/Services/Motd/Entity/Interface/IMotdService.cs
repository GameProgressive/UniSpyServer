using RetroSpyServices.Motd.Entity.Structure.Model;
using System.ServiceModel;

namespace RetroSpyServices.Motd.Entity.Interface
{
    [ServiceContract]
    internal interface IMotdService
    {
        [OperationContract]
        string Test(string s);

        [OperationContract]
        void XmlMethod(System.Xml.Linq.XElement xml);

        [OperationContract]
        MotdServiceModel TestMotdServiceModel(MotdServiceModel inputModel);
    }
}