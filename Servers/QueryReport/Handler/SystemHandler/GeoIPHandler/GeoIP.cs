using GameSpyLib.Logging;
using MaxMind.GeoIP2;
using MaxMind.GeoIP2.Responses;
using System;
using System.IO;
using System.Net;

namespace QueryReport.DatabaseQuery
{
    public static class GeoIP
    {
        /// <summary>
        /// MaxMind GeoIP2 Reader object
        /// </summary>
        private static DatabaseReader Reader { get; set; }

        static GeoIP()
        {
            try
            {
                // Dont attempt to create, just quit
                string file = Path.Combine("Common files", "GeoLite2-Country.mmdb");
                if (!File.Exists(file))
                    LogWriter.Log.Write("[GeoIP] GeoLite2-Country.mmdb file is missing!", LogLevel.Fatal);

                Reader = new DatabaseReader(file);
            }
            catch (Exception e)
            {
                LogWriter.Log.Write(LogLevel.Warning, "[GeoIP] {0}", e.Message);
            }
        }

        /// <summary>
        /// Gets the country code for a string IP address
        /// </summary>
        /// <param name="IP"></param>
        /// <returns></returns>
        public static string GetCountryCode(IPAddress IP)
        {
            // Try and get country code
            CountryResponse response;
            if (!Reader.TryCountry(IP, out response))
            {
                return "??";
            }

            return response.Country.IsoCode;
        }

        /// <summary>
        /// Fethces the full country name from a country code supplied from GetCountryCode()
        /// </summary>
        /// <param name="Code"></param>
        /// <returns></returns>
        public static string GetCountyName(IPAddress IP)
        {
            // Try and get country code
            CountryResponse response;
            if (!Reader.TryCountry(IP, out response))
            {
                return "??";
            }

            return response.Country.Name;
        }

        public static void Exit()
        {
            Reader.Dispose();
        }
    }
}
