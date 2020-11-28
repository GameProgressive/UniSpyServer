using RetroSpyServices.Competitive.Entity.Structure.Model;
using System.ServiceModel;
using System.Xml.Linq;

namespace RetroSpyServices.Competitive.Entity.Interface
{
    [ServiceContract]
    public interface ICompetitiveService
    {
        [OperationContract]
        string Test(string s);

        [OperationContract]
        void XmlMethod(XElement xml);

        [OperationContract]
        CompetitiveServiceModel TestCompetitiveServiceModel(CompetitiveServiceModel inputModel);
    }
}