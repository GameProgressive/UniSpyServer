using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace UniSpyLib.MiscMethod
{
    public class IPAddressConverter : JsonConverter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public override bool CanConvert(Type objectType)
        {
            if (objectType == typeof(IPAddress)) return true;
            if (objectType == typeof(List<IPAddress>)) return true;

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="objectType"></param>
        /// <param name="existingValue"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // convert an ipaddress represented as a string into an IPAddress object and return it to the caller
            if (objectType == typeof(IPAddress))
            {
                return IPAddress.Parse(JToken.Load(reader).ToString());
            }

            // convert a json array of ipaddresses represented as strings into a List<IPAddress> object and return it to the caller
            if (objectType == typeof(List<IPAddress>))
            {
                return JToken.Load(reader).Select(address => IPAddress.Parse((string)address)).ToList();
            }

            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="serializer"></param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            // convert an IPAddress object to a string representation of itself and write it to the serialiser
            if (value.GetType() == typeof(IPAddress))
            {
                JToken.FromObject(value.ToString()).WriteTo(writer);
                return;
            }

            // convert a List<IPAddress> object to an array of strings of ipaddresses and write it to the serialiser
            if (value.GetType() == typeof(List<IPAddress>))
            {
                JToken.FromObject((from n in (List<IPAddress>)value select n.ToString()).ToList()).WriteTo(writer);
                return;
            }

            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class IPEndPointConverter : JsonConverter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(IPEndPoint);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="objectType"></param>
        /// <param name="existingValue"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return JToken.Load(reader).ToString().ToIPEndPoint();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="serializer"></param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var ipEndPoint = value as IPEndPoint;
            if (ipEndPoint != null)
            {
                if (ipEndPoint.Address != null || ipEndPoint.Port != 0)
                {
                    JToken.FromObject(string.Format("{0}:{1}", ipEndPoint.Address, ipEndPoint.Port)).WriteTo(writer);
                    return;
                }
            }
            writer.WriteNull();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class IPAddressExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ipEndPoint"></param>
        /// <returns></returns>
        public static IPEndPoint ToIPEndPoint(this string ipEndPoint)
        {
            if (string.IsNullOrWhiteSpace(ipEndPoint))
            {
                return null;
            }

            var components = ipEndPoint.Split(':');

            return new IPEndPoint(IPAddress.Parse(components[0]), Convert.ToInt32(components[1]));
        }

    }

    public static class UniSpyJsonConverter
    {
        public static async Task Initialize()
        {
            var settings = new JsonSerializerSettings();
            settings.Converters.Add(new IPAddressConverter());
            settings.Converters.Add(new IPEndPointConverter());
            settings.Formatting = Formatting.Indented;
            JsonConvert.DefaultSettings = () => settings;
        }
    }
}
