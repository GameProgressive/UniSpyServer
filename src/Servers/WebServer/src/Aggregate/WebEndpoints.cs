using System.Collections.Generic;

namespace UniSpy.Server.WebServer.Aggregate
{
    public class WebEndpoints
    {
        //Contains all endpoints of the web server
        public static readonly List<string> AvailableEndpoints = new List<string>
        {
            // WSMotd Service
            "/motd/motd.asp",
            "/motd/vercheck.asp",
            // WSAuth Service
            "/AuthService/AuthService.asmx",
            // WSSake Service
            "/SakeStorageServer/StorageServer.asmx",
            "/SakeFileServer/download.aspx",
            "/SakeFileServer/upload.aspx",
            // WSAtlas Service
            "/CompetitionService/CompetitionService.asmx",
            // WSDirect2Game Service
            "/commerce/1.1/catalogservice.svc",
            "/commerce/1.1/accountservice.svc",
            "/commerce/1.1/purchaseservice.svc",
            // WSIngameAd Service
            "/IGNAdServer/service1.asmx",
            // WSRacing Service
            "/RaceService/NintendoRacingService.asmx",
        };
    }
}