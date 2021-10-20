using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UniSpyServer.WebServer.Abstraction;
using UniSpyServer.WebServer.Entity.Contract;

namespace UniSpyServer.WebServer.Entity.Structure.Request.PatchingAndTrackingRequest
{
    [RequestContract("Motd")]
    public class MotdRequest : RequestBase
    {
        public override void Parse()
        {
            throw new System.NotImplementedException();
        }
    }
}