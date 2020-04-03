using System.ServiceModel;
namespace RetroSpyServices.Authentication
{
    [ServiceContract]
    public interface IAuthService
    {
        [OperationContract]
        string LoginUniqueNick(string s);

        [OperationContract]
        void XmlMethod(System.Xml.Linq.XElement xml);
        [OperationContract]
        AuthServiceModel TestAuthServiceModel(AuthServiceModel inputModel);
    }
}