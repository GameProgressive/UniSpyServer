using UniSpyServer.Servers.WebServer.Abstraction;
using UniSpyServer.Servers.WebServer.Entity.Contract;

namespace UniSpyServer.Servers.WebServer.Entity.Structure.Request.PatchingAndTracking
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