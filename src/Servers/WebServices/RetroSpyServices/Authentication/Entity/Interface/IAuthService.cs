using RetroSpyServices.Authentication.Entity.Structure.Model;
using System.ServiceModel;
using System.Xml.Linq;

namespace RetroSpyServices.Authentication.Entity.Interface
{
    [ServiceContract]
    public interface IAuthService
    {
        [OperationContract]
        string LoginUniqueNick(string s);

        [OperationContract]
        void XmlMethod(XElement xml);

        [OperationContract]
        AuthServiceModel TestAuthServiceModel(AuthServiceModel inputModel);

    }
}