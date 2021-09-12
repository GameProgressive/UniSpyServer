// using System;
// using System.Threading.Tasks;
// using UniSpyLib.Abstraction.BaseClass;
// using UniSpyLib.Abstraction.BaseClass.Factory;
// using UniSpyLib.Logging;

// namespace WebServer.Application
// {
//     internal class Program
//     {
//         static void Main(string[] args)
//         {
//             try
//             {
//                 new ServerFactory().Start();
//             }
//             catch (Exception e)
//             {
//                 LogWriter.ToLog(e);
//             }

//             Console.WriteLine("Press < Q > to exit. ");
//             while (Console.ReadKey().Key != ConsoleKey.Q) { }
//         }
//     }
// }


using System;
using WebServer.Entity.Structure.Request;
var rawRequest = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><SOAP-ENV:Envelope xmlns:SOAP-ENV=\"http://schemas.xmlsoap.org/soap/envelope/\"    xmlns:SOAP-ENC=\"http://schemas.xmlsoap.org/soap/encoding/\"    xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"    xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:ns1=\"http://gamespy.net/sake\"><SOAP-ENV:Body><ns1:GetMyRecords><ns1:gameid>1687</ns1:gameid><ns1:secretKey>9r3Rmy</ns1:secretKey><ns1:loginTicket>xxxxxxxx_YYYYYYYYYY__</ns1:loginTicket><ns1:tableid>FriendInfo</ns1:tableid><ns1:fields><ns1:string>info</ns1:string><ns1:string>recordid</ns1:string></ns1:fields></ns1:GetMyRecords></SOAP-ENV:Body></SOAP-ENV:Envelope>";
var request = new GetMyRecordRequest(rawRequest);
request.Parse();
Console.WriteLine(request.ToString());