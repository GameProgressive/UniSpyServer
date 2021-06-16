using RetroSpyServices.Direct2Game.Entity.Structure.Model;
using System.ServiceModel;

namespace RetroSpyServices.Direct2Game.Entity.Interface
{
    [ServiceContract]
    internal interface IDirect2GameService
    {
        [OperationContract]
        string Test(string s);

        [OperationContract]
        void XmlMethod(System.Xml.Linq.XElement xml);

        [OperationContract]
        Direct2GameServiceModel TestDirect2GameServiceModel(Direct2GameServiceModel inputModel);
    }
}