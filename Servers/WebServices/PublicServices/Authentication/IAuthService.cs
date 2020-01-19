using System.ServiceModel;
namespace PublicServices.Authentication
{
    [ServiceContract]
    public interface IAuthService
    {
        [OperationContract]
        string Test(string s);
        [OperationContract]
        void XmlMethod(System.Xml.Linq.XElement xml);
        [OperationContract]
        AuthServiceModel TestAuthServiceModel(AuthServiceModel inputModel);
    }
}