using System.ServiceModel;
namespace Sake
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