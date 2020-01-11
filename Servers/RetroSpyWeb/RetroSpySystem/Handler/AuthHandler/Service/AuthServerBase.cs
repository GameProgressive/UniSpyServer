//using Handler.AuthHandler.Struct;
//using System.ServiceModel;
//using System.Threading.Tasks;

//namespace Handler.AuthHandler.Service
//{
//    [ServiceContract]
//    public interface AuthServerBase
//    {
//        [OperationContract]
//        string Ping(string s);

//        [OperationContract]
//        AuthenticationResponse PingComplexModel(AuthenticationRequest clientRequest);

//        [OperationContract]
//        void VoidMethod(out string s);

//        [OperationContract]
//        Task<int> AsyncMethod();

//        [OperationContract]
//        int? NullableMethod(bool? arg);
//    }
//}

