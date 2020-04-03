using System.ServiceModel;
namespace PublicServices.Direct2Game
{
    [ServiceContract]
    public interface IDirect2GameService
    {
        [OperationContract]
        string Test(string s);
        [OperationContract]
        void XmlMethod(System.Xml.Linq.XElement xml);
        [OperationContract]
        Direct2GameServiceModel TestDirect2GameServiceModel(Direct2GameServiceModel inputModel);
    }
}