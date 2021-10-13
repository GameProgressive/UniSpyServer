using System.Collections.Generic;

namespace WebServer.Entity.Constant
{
    public class WebEndpoints
    {
        //Contains all the endpoints of the web server
        public static readonly List<string> AvailableEndpoints = new List<string>
        {
            "/CompetitionService/CompetitionService.asmx",
            "/AuthService/AuthService.asmx",
            "/RaceService/NintendoRacingService.asmx",
            "/SakeStorageServer/StorageServer.asmx",
        };
    }
}