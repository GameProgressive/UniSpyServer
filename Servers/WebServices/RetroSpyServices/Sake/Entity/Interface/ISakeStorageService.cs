using System.ServiceModel;
using System.Xml.Linq;
using RetroSpyServices.Sake.Entity.Structure.Model;

namespace RetroSpyServices.Sake.Entity.Interface
{
    [ServiceContract]
    public interface ISakeStorageService
    {
        [OperationContract]
        string Test(string s);

        string Upload();

        [OperationContract]
        void XmlMethod(XElement xml);

        [OperationContract]
        SakeStorageModel TestStorageServerModel(SakeStorageModel inputModel);
    }
}