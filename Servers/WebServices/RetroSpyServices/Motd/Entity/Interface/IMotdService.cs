using System.ServiceModel;
using RetroSpyServices.Motd.Entity.Structure.Model;

namespace RetroSpyServices.Motd.Entity.Interface
{
    [ServiceContract]
    public interface IMotdService
    {
        [OperationContract]
        string Test(string s);

        [OperationContract]
        void XmlMethod(System.Xml.Linq.XElement xml);

        [OperationContract]
        MotdServiceModel TestMotdServiceModel(MotdServiceModel inputModel);
    }
}