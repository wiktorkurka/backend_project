using Newtonsoft.Json;
using System.Net;

namespace UniIMP.Utility.JsonConverters
{
    public class IpAddressConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            if (objectType == typeof(IPAddress)) return true;

            return false;
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            if (reader.Value == null)
                return null;

            IPAddress? addr;
            IPAddress.TryParse((string)reader.Value, out addr);
            return addr;
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            if (value is IPAddress)
                writer.WriteValue(value.ToString());
        }
    }
}