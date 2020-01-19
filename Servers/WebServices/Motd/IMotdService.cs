using System.ServiceModel;
namespace Motd
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