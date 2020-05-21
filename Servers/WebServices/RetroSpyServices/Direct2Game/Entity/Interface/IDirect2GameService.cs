using System.ServiceModel;
using RetroSpyServices.Direct2Game.Entity.Structure.Model;

namespace RetroSpyServices.Direct2Game.Entity.Interface
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