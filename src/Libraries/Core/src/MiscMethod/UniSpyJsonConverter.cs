using System;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace UniSpy.Server.Core.MiscMethod
{

    public class IPAddressConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(IPAddress));
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return IPAddress.Parse((string)reader.Value);
        }
    }
    public class IPAddresConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(IPAddress));
        }
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            IPAddress ep = (IPAddress)value;
            JObject jo = new JObject();
            jo.Add("Address", JToken.FromObject(ep.ToString(), serializer));
            jo.WriteTo(writer);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jo = JObject.Load(reader);
            IPAddress address = IPAddress.Parse(jo["Address"].ToObject<string>());
            return address;
        }
    }
    public class IPEndPointConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(IPEndPoint));
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            IPEndPoint ep = (IPEndPoint)value;
            JObject jo = new JObject();
            jo.Add("Address", JToken.FromObject(ep.Address.ToString(), serializer));
            jo.Add("Port", ep.Port);
            jo.WriteTo(writer);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // if (reader.Value == null)
            // {
            //     return null;
            // }
            JObject jo = JObject.Load(reader);
            IPAddress address = IPAddress.Parse(jo["Address"].ToObject<string>());
            int port = (int)jo["Port"];
            return new IPEndPoint(address, port);
        }
    }

}
