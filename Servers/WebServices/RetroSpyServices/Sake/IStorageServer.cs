using System.ServiceModel;
namespace RetroSpyServices.Sake
{
    [ServiceContract]
    public interface IStorageServer
    {
        [OperationContract]
        string Test(string s);

        string upload();

        [OperationContract]
        void XmlMethod(System.Xml.Linq.XElement xml);
        [OperationContract]
        StorageServerModel TestStorageServerModel(StorageServerModel inputModel);
    }
}