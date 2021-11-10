using System.Collections.Generic;

namespace UniSpyServer.Servers.WebServer.Entity.Constant
{
    public class WebEndpoints
    {
        //Contains all endpoints of the web server
        public static readonly List<string> AvailableEndpoints = new List<string>
        {
            "/motd/motd.asp",
            "/motd/vercheck.asp",
            "/AuthService/AuthService.asmx",
            "/SakeStorageServer/StorageServer.asmx",
            "/CompetitionService/CompetitionService.asmx",
            "/RaceService/NintendoRacingService.asmx",
        };
    }
}