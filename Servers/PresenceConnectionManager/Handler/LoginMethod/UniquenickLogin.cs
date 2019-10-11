using System;
using System.Collections.Generic;
using System.Text;

namespace PresenceConnectionManager.Handler.LoginMethod
{
    /// <summary>
    /// This class is only for login logical separation
    /// </summary>
    public class UniquenickLogin
    {
        public static void Login()
        {
            if (LoginHandler.Recv.ContainsKey("namespaceid"))
            {
                if (LoginHandler.Recv["namespaceid"] == " 1")
                {
                    DefaultNamespaceLogin();
                }
                if (LoginHandler.Recv["namespaceid"] != " 1")
                {
                    CustomNamespaceLogin();
                }
            }
            
        }

        private static void CustomNamespaceLogin()
        {
            throw new NotImplementedException();
        }

        private static void DefaultNamespaceLogin()
        {
            throw new NotImplementedException();
        }



//        switch (Convert.ToInt32(LoginHandler.Recv["sdkrevision"]))
//                {
//                    case GameSpySDKRevision.Type1:
//                        TypeOneLogin();
//                        break;
//                    case GameSpySDKRevision.Type2:
//                        TypeTwoLogin();
//                        break;
//                    case GameSpySDKRevision.Type3:
//                        TypeTreeLogin();
//                        break;
//                    case GameSpySDKRevision.Type4:
//                        TypeFourLogin();
//                        break;
//                }
//}
//            else
//            {

//            }
//        }

//         private static void TypeFourLogin()
//{
//    throw new NotImplementedException();
//}

//private static void TypeTreeLogin()
//{
//    throw new NotImplementedException();
//}

//private static void TypeTwoLogin()
//{
//    throw new NotImplementedException();
//}

//private static void TypeOneLogin()
//{
//    throw new NotImplementedException();
//}
    }
}
