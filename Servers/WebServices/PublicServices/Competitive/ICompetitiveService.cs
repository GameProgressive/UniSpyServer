using System.ServiceModel;
namespace PublicServices.Competitive
{
    [ServiceContract]
    public interface ICompetitiveService
    {
        [OperationContract]
        string Test(string s);
        [OperationContract]
        void XmlMethod(System.Xml.Linq.XElement xml);
        [OperationContract]
        CompetitiveServiceModel TestCompetitiveServiceModel(CompetitiveServiceModel inputModel);
    }
}