using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UniSpyServer.Servers.WebServer.Abstraction;
using UniSpyServer.Servers.WebServer.Entity.Contract;

namespace UniSpyServer.Servers.WebServer.Entity.Structure.Request.PatchingAndTrackingRequest
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