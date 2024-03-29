// using System;
// using System.Linq;
// using System.Net;
// using UniSpy.Server.NatNegotiation.Abstraction.BaseClass;
// 
// using UniSpy.Server.NatNegotiation.Enumerate;

// namespace UniSpy.Server.NatNegotiation.Contract.Request
// {
//     /// <summary>
//     /// When client can not start p2p game, it will use our server to redirect his game traffic
//     /// </summary>
//     
//     public sealed class PingRequest : CommonRequestBase
//     {
//         public IPEndPoint GuessedTargetIPEndPoint { get; set; }
//         public byte? GotYourData { get; private set; }
//         public byte? IsFinished { get; private set; }
//         public PingRequest(byte[] rawRequest) : base(rawRequest)
//         {
//         }
//         public override void Parse()
//         {
//             base.Parse();
//             var _ipBytes = RawRequest.Skip(12).Take(4).ToArray();
//             var _portBytes = RawRequest.Skip(16).Take(2).ToArray();
//             GuessedTargetIPEndPoint = new IPEndPoint(new IPAddress(_ipBytes), BitConverter.ToUInt16(_portBytes));
//             GotYourData = RawRequest[18];
//             IsFinished = RawRequest[19];
//         }
//     }
// }